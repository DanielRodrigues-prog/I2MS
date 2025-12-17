using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Windows.Forms;
using QRCoder; // Garante que tem QRCoder instalado

namespace WindowsFormsApp2
{
    public class GeradorReciboV2
    {
        private List<DataGridViewRow> _ferramentas;
        private string _cedNome, _cedEmpresa, _cedEmail, _cedTel;
        private string _reqNome, _reqEmpresa, _reqEmail, _reqTel;
        private string _observacao;
        private string _codigoGerado;

        // Controle de Paginação
        private int _indiceItemAtual = 0; // Guarda onde parou a lista

        // Cores
        private readonly Color _corPrimaria = Color.FromArgb(41, 128, 185);
        private readonly Color _corSecundaria = Color.FromArgb(52, 152, 219);
        private readonly Color _corAcento = Color.FromArgb(230, 126, 34);
        private readonly Color _corSucesso = Color.FromArgb(46, 204, 113);
        private readonly Color _corTexto = Color.FromArgb(44, 62, 80);

        public GeradorReciboV2(List<DataGridViewRow> ferramentas,
                             string cNome, string cEmp, string cEmail, string cTel,
                             string rNome, string rEmp, string rEmail, string rTel,
                             string observacao)
        {
            _ferramentas = ferramentas ?? new List<DataGridViewRow>();
            _cedNome = cNome; _cedEmpresa = cEmp; _cedEmail = cEmail; _cedTel = cTel;
            _reqNome = rNome; _reqEmpresa = rEmp; _reqEmail = rEmail; _reqTel = rTel;
            _observacao = observacao;

            var now = DateTime.Now;
            _codigoGerado = $"EMP-{now:yyyyMMdd}-{now:HHmmss}";
        }

        public void Imprimir()
        {
            // ZERA O CONTADOR ANTES DE COMEÇAR
            _indiceItemAtual = 0;

            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.Landscape = true; // Paisagem
            pd.PrintPage += Pd_PrintPage;

            PrintPreviewDialog ppd = new PrintPreviewDialog();
            ppd.Document = pd;
            ppd.Width = 1000;
            ppd.Height = 800;
            ppd.ShowDialog();
        }

        private void Pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            // Configurações da Página
            int margin = 25;
            int pageWidth = e.PageBounds.Width - (margin * 2);
            int pageHeight = e.PageBounds.Height - margin; // Limite inferior
            int currentY = margin;
            int x = margin;

            // =====================================================
            // 1. CABEÇALHO (Repete em todas as páginas)
            // =====================================================
            int headerHeight = 140;
            Rectangle rectHeader = new Rectangle(x, currentY, pageWidth, headerHeight);

            using (LinearGradientBrush brush = new LinearGradientBrush(rectHeader, _corPrimaria, _corSecundaria, 45f))
            {
                g.FillRectangle(brush, rectHeader);
            }

            // LOGO
            string caminhoLogo = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "voaraviation_logo.png");
            int logoSize = 80;
            int logoY = currentY + (headerHeight - logoSize) / 2;
            Rectangle logoRect = new Rectangle(x + 30, logoY, logoSize, logoSize);
            g.FillEllipse(Brushes.White, logoRect);

            if (File.Exists(caminhoLogo))
            {
                try
                {
                    using (Image img = Image.FromFile(caminhoLogo))
                        g.DrawImage(img, x + 40, logoY + 10, logoSize - 20, logoSize - 20);
                }
                catch { DesenharTextoLogo(g, logoRect); }
            }
            else { DesenharTextoLogo(g, logoRect); }

            // TÍTULOS
            float textoX = x + logoSize + 50;
            g.DrawString("RECIBO DE EMPRÉSTIMO", new Font("Segoe UI", 24, FontStyle.Bold), Brushes.White, textoX, currentY + 35);
            g.DrawString("Ferramentas e Equipamentos", new Font("Segoe UI", 12), Brushes.WhiteSmoke, textoX, currentY + 80);

            // QR CODE
            Bitmap qrCode = GerarQRCode(_codigoGerado);
            if (qrCode != null)
            {
                int qrSize = 90;
                int qrX = x + pageWidth - qrSize - 20;
                int qrY = currentY + 15;
                g.FillRectangle(Brushes.White, qrX - 5, qrY - 5, qrSize + 10, qrSize + 10);
                g.DrawImage(qrCode, qrX, qrY, qrSize, qrSize);

                // Código escrito
                Font fCode = new Font("Segoe UI", 7, FontStyle.Bold);
                SizeF sCode = g.MeasureString(_codigoGerado, fCode);
                g.DrawString(_codigoGerado, fCode, Brushes.White, qrX + (qrSize - sCode.Width) / 2, qrY + qrSize + 5);
            }

            currentY += headerHeight + 30;

            // =====================================================
            // 2. CARDS (Apenas na primeira página, opcional)
            // =====================================================
            // Se quiser que apareça só na pág 1: if (_indiceItemAtual == 0) { ... }
            // Vou manter em todas para garantir o layout, mas se faltar espaço podemos mudar.

            int cardHeight = 130;
            int cardWidth = (pageWidth - 20) / 2;

            DesenharCard(g, "CEDENTE (Quem Empresta)", new string[] { _cedNome, _cedEmpresa, _cedEmail, _cedTel }, x, currentY, cardWidth, cardHeight, _corSucesso);
            DesenharCard(g, "REQUERENTE (Quem Recebe)", new string[] { _reqNome, _reqEmpresa, _reqEmail, _reqTel }, x + cardWidth + 20, currentY, cardWidth, cardHeight, _corAcento);

            currentY += cardHeight + 30;

            // =====================================================
            // 3. TABELA DE ITENS (Com Paginação Segura)
            // =====================================================
            Font fontHeader = new Font("Segoe UI", 9, FontStyle.Bold);
            Font fontDados = new Font("Segoe UI", 8);

            // Cabeçalho da Tabela
            int colItem = 40;
            int colNome = pageWidth - 440;
            int colFixa = 100;
            int[] larguras = { colItem, colNome, colFixa, colFixa, colFixa, colFixa };
            string[] titulos = { "ITEM", "NOME", "ID VOAR", "P/N", "S/N", "VALIDADE" };
            int rowHeight = 35;

            Rectangle rectHeaderTabela = new Rectangle(x, currentY, pageWidth, rowHeight);
            using (LinearGradientBrush brush = new LinearGradientBrush(rectHeaderTabela, Color.FromArgb(52, 73, 94), Color.FromArgb(44, 62, 80), 0f))
            {
                g.FillRectangle(brush, rectHeaderTabela);
            }

            int cx = x;
            for (int i = 0; i < titulos.Length; i++)
            {
                g.DrawString(titulos[i], fontHeader, Brushes.White, new Rectangle(cx + 5, currentY, larguras[i], rowHeight), new StringFormat { LineAlignment = StringAlignment.Center });
                cx += larguras[i];
            }
            currentY += rowHeight;

            // --- LOOP DOS ITENS ---
            bool itensFinalizados = true;

            // Começa do índice salvo (_indiceItemAtual) e não do zero!
            for (int i = _indiceItemAtual; i < _ferramentas.Count; i++)
            {
                var row = _ferramentas[i];

                // Verifica se cabe na página (Deixa 180px de folga para assinaturas/rodapé)
                if (currentY + rowHeight > pageHeight - 180)
                {
                    e.HasMorePages = true; // Avisa que tem mais página
                    itensFinalizados = false; // Não terminou a lista
                    return; // PARA AQUI e volta na próxima chamada
                }

                // Desenha a linha
                string nome = ObterValor(row, "colInstrumento", "colSemDescricao");
                string id = ObterValor(row, "colIdentifSOD", "colSemCodigo");
                string pn = ObterValor(row, "colPN", "colSemPN");
                string sn = ObterValor(row, "colSN", "-");
                string val = ObterValor(row, "colDataVencimento", "-");

                if ((i + 1) % 2 == 0) g.FillRectangle(new SolidBrush(Color.FromArgb(250, 250, 250)), x, currentY, pageWidth, rowHeight);

                cx = x;
                g.DrawString((i + 1).ToString(), fontDados, new SolidBrush(_corTexto), new Rectangle(cx + 5, currentY, larguras[0], rowHeight), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center }); cx += larguras[0];
                g.DrawString(TruncarTexto(nome, 60), fontDados, new SolidBrush(_corTexto), new Rectangle(cx + 5, currentY, larguras[1] - 10, rowHeight), new StringFormat { LineAlignment = StringAlignment.Center }); cx += larguras[1];

                string[] dadosLinha = { id, pn, sn, val };
                for (int k = 0; k < dadosLinha.Length; k++)
                {
                    g.DrawString(dadosLinha[k], fontDados, new SolidBrush(_corTexto), new Rectangle(cx + 5, currentY, larguras[k + 2], rowHeight), new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Center });
                    cx += larguras[k + 2];
                }

                g.DrawLine(new Pen(Color.FromArgb(220, 220, 220), 1), x, currentY + rowHeight, x + pageWidth, currentY + rowHeight);
                currentY += rowHeight;

                _indiceItemAtual++; // Incrementa para saber que já imprimiu este
            }

            // Se saiu do loop, imprimiu todos os itens.
            currentY += 20;

            // =====================================================
            // 4. OBSERVAÇÕES E ASSINATURAS (Só se couber ou nova página)
            // =====================================================

            // Verifica se tem espaço para assinaturas (precisa de uns 150px)
            if (currentY + 150 > pageHeight)
            {
                e.HasMorePages = false; // Cria uma nova página só para assinaturas
                return;
            }

            // --- OBSERVAÇÕES ---
            g.DrawString("OBSERVAÇÕES", new Font("Segoe UI", 11, FontStyle.Bold), new SolidBrush(_corTexto), x, currentY);
            currentY += 25;

            Rectangle rectObs = new Rectangle(x, currentY, pageWidth, 50);
            g.FillRectangle(new SolidBrush(Color.FromArgb(245, 245, 245)), rectObs);
            g.DrawRectangle(new Pen(Color.FromArgb(220, 220, 220)), rectObs);

            string obsStr = string.IsNullOrEmpty(_observacao) ? "Sem observações." : _observacao;
            g.DrawString(obsStr, new Font("Segoe UI", 9), new SolidBrush(_corTexto), new RectangleF(x + 10, currentY + 10, pageWidth - 20, 30));

            currentY += 80;

            // --- ASSINATURAS ---
            int signWidth = (pageWidth - 40) / 2;

            // Cedente
            g.DrawLine(new Pen(_corTexto, 2), x, currentY + 40, x + signWidth, currentY + 40);
            g.DrawString("Assinatura do Cedente", new Font("Segoe UI", 8), new SolidBrush(_corTexto), x, currentY + 45);
            g.DrawString($"Data: {DateTime.Now:dd/MM/yyyy}", new Font("Segoe UI", 7), Brushes.Gray, x, currentY + 60);

            // Requerente
            g.DrawLine(new Pen(_corTexto, 2), x + signWidth + 40, currentY + 40, x + pageWidth, currentY + 40);
            g.DrawString("Assinatura do Requerente", new Font("Segoe UI", 8), new SolidBrush(_corTexto), x + signWidth + 40, currentY + 45);
            g.DrawString($"Data: {DateTime.Now:dd/MM/yyyy}", new Font("Segoe UI", 7), Brushes.Gray, x + signWidth + 40, currentY + 60);

            // --- RODAPÉ ---
            int footerY = e.PageBounds.Height - 30;
            g.DrawLine(new Pen(Color.LightGray, 1), margin, footerY, e.PageBounds.Width - margin, footerY);
            g.DrawString("Comprovante gerado pelo Sistema de Controle de Ferramentas VOAR.", new Font("Segoe UI", 7), Brushes.Gray, margin, footerY + 5);

            // Se chegou aqui, terminou tudo
            e.HasMorePages = false;
            _indiceItemAtual = 0; // Reseta para próxima impressão
        }

        // --- HELPER METHODS ---
        private void DesenharTextoLogo(Graphics g, Rectangle r)
        {
            StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString("VOAR", new Font("Arial", 14, FontStyle.Bold), new SolidBrush(_corPrimaria), r, sf);
        }

        private void DesenharCard(Graphics g, string titulo, string[] linhas, int x, int y, int w, int h, Color cor)
        {
            g.FillRectangle(new SolidBrush(Color.FromArgb(30, 0, 0, 0)), new Rectangle(x + 3, y + 3, w, h));
            Rectangle rect = new Rectangle(x, y, w, h);
            g.FillRectangle(Brushes.White, rect);
            g.DrawRectangle(new Pen(Color.FromArgb(220, 220, 220), 2), rect);
            g.FillRectangle(new SolidBrush(cor), new Rectangle(x, y, w, 8));

            Font fontTitulo = new Font("Segoe UI", 10, FontStyle.Bold);
            g.DrawString(titulo, fontTitulo, new SolidBrush(_corTexto), x + 15, y + 20);

            Font fontDados = new Font("Segoe UI", 8);
            int yDados = y + 50;
            foreach (string linha in linhas) { g.DrawString(linha, fontDados, Brushes.Gray, x + 15, yDados); yDados += 18; }
        }

        private Bitmap GerarQRCode(string texto)
        {
            try { return new QRCode(new QRCodeGenerator().CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q)).GetGraphic(4); } catch { return null; }
        }

        private string ObterValor(DataGridViewRow row, string c1, string c2)
        {
            if (row.DataGridView != null)
            {
                if (row.DataGridView.Columns.Contains(c1)) return row.Cells[c1].Value?.ToString() ?? "-";
                if (row.DataGridView.Columns.Contains(c2)) return row.Cells[c2].Value?.ToString() ?? "-";
            }
            return "-";
        }

        private string TruncarTexto(string texto, int maxLen)
        {
            if (string.IsNullOrEmpty(texto)) return "";
            return texto.Length <= maxLen ? texto : texto.Substring(0, maxLen - 3) + "...";
        }
    }
}