using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public class GeradorRecibo
    {
        private List<DataGridViewRow> _ferramentas;
        private string _cedNome, _cedEmpresa, _cedEmail, _cedTel;
        private string _reqNome, _reqEmpresa, _reqEmail, _reqTel;
        private string _observacao;
        private string _usuarioLogado;
        private string _codigoGerado;

        // Cores e Fontes personalizadas para ficar igual ao design
        private readonly Color _corAzulVoa = Color.FromArgb(0, 0, 139); // Azul escuro estilo "VOA"
        private readonly Font _fontLogo = new Font("Arial", 28, FontStyle.Bold);
        private readonly Font _fontTitulo = new Font("Arial", 14, FontStyle.Bold);
        private readonly Font _fontLabel = new Font("Arial", 7, FontStyle.Regular); // Letra miuda (Ex: Responsável)
        private readonly Font _fontValor = new Font("Arial", 8, FontStyle.Bold);    // Letra valor (Ex: Manutenção)
        private readonly Font _fontTabelaHeader = new Font("Arial", 7, FontStyle.Bold);
        private readonly Font _fontTabelaDados = new Font("Arial", 7, FontStyle.Regular);

        public GeradorRecibo(List<DataGridViewRow> ferramentas,
                             string cNome, string cEmp, string cEmail, string cTel,
                             string rNome, string rEmp, string rEmail, string rTel,
                             string observacao, string usuarioLogado = "")
        {
            _ferramentas = ferramentas;
            _cedNome = cNome; _cedEmpresa = cEmp; _cedEmail = cEmail; _cedTel = cTel;
            _reqNome = rNome; _reqEmpresa = rEmp; _reqEmail = rEmail; _reqTel = rTel;
            _observacao = observacao;
            _usuarioLogado = usuarioLogado;

            // Gera código: EMP.ANO.MES.DIA.SEGUNDO
            var now = DateTime.Now;
            _codigoGerado = $"EMP.{now.Year}.{now.Month}.{now.Day}.{now.Second}";
        }

        public void Imprimir()
        {
            PrintDocument pd = new PrintDocument();
            // O formulário é largo, Paisagem acomoda melhor a tabela
            pd.DefaultPageSettings.Landscape = true;
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
            // Configurações de desenho
            Pen penBorda = new Pen(Color.Black, 1);
            Brush brushTexto = Brushes.Black;

            // Margens e Áreas
            int margin = 30;
            int pageWidth = e.PageBounds.Width - (margin * 2);
            int currentY = margin;
            int x = margin;

            // ==========================================================
            // 1. CABEÇALHO (Igual ao PDF/Imagem)
            // ==========================================================
            int heightHeader = 70;

            // Retângulo Principal do Cabeçalho
            g.DrawRectangle(penBorda, x, currentY, pageWidth, heightHeader);

            // Larguras das 3 seções do cabeçalho
            int wLogo = (int)(pageWidth * 0.15);      // 15% para Logo
            int wInfo = (int)(pageWidth * 0.25);      // 25% para Info Técnica (Direita)
            int wTitulo = pageWidth - wLogo - wInfo;  // Resto para o Título

            // Linhas verticais separando as seções
            g.DrawLine(penBorda, x + wLogo, currentY, x + wLogo, currentY + heightHeader);
            g.DrawLine(penBorda, x + wLogo + wTitulo, currentY, x + wLogo + wTitulo, currentY + heightHeader);

            // --- A. LOGO (Esquerda) ---
            // Desenha "VOA" em Azul
            StringFormat sfCenter = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
            g.DrawString("VOAR", _fontLogo, new SolidBrush(_corAzulVoa), new Rectangle(x, currentY, wLogo, heightHeader), sfCenter);

            // --- B. TÍTULO (Centro) ---
            string tituloTexto = "EMPRÉSTIMO DE EQUIPAMENTOS E\nFERRAMENTAS";
            g.DrawString(tituloTexto, _fontTitulo, brushTexto, new Rectangle(x + wLogo, currentY, wTitulo, heightHeader), sfCenter);

            // --- C. INFO TÉCNICA (Direita - Grade 3x2) ---
            int xInfo = x + wLogo + wTitulo;
            int hLine = heightHeader / 3;
            int wCol1 = wInfo / 2;

            // Linhas Horizontais da grade direita
            g.DrawLine(penBorda, xInfo, currentY + hLine, x + pageWidth, currentY + hLine);
            g.DrawLine(penBorda, xInfo, currentY + (hLine * 2), x + pageWidth, currentY + (hLine * 2));
            // Linha Vertical central da grade direita
            g.DrawLine(penBorda, xInfo + wCol1, currentY, xInfo + wCol1, currentY + heightHeader);

            // Preenchimento dos dados do cabeçalho (Label pequena em cima, Valor negrito embaixo)
            DrawHeaderCell(g, "Responsável\nManutenção", "", xInfo, currentY, wCol1, hLine);
            DrawHeaderCell(g, "Formulário", "", xInfo + wCol1, currentY, wCol1, hLine);

            DrawHeaderCell(g, "Código", _codigoGerado, xInfo, currentY + hLine, wCol1, hLine);
            DrawHeaderCell(g, "Classificação", "DI – Domínio Interno", xInfo + wCol1, currentY + hLine, wCol1, hLine);

            DrawHeaderCell(g, "Revisão", "N/A", xInfo, currentY + (hLine * 2), wCol1, hLine);
            DrawHeaderCell(g, "Data", DateTime.Now.ToString("dd/MM/yyyy"), xInfo + wCol1, currentY + (hLine * 2), wCol1, hLine);

            currentY += heightHeader + 10; // Espaço após cabeçalho

            // ==========================================================
            // 2. TABELA DE ITENS
            // ==========================================================

            // Definição das larguras das colunas (Soma = pageWidth)
            int c1 = 30;  // ITEM
            int c3 = 100; // ID VOAR
            int c4 = 80;  // PN
            int c5 = 80;  // SN
            int c6 = 90;  // VALIDADE
            int c7 = 100; // COND CEDENTE
            int c8 = 100; // COND REQUERENTE
            int c2 = pageWidth - (c1 + c3 + c4 + c5 + c6 + c7 + c8); // NOME (Pega o resto)

            int hRowHeaderTop = 20; // Barra "CEDENTE / REQUERENTE"
            int hRowHeader = 35;    // Barra dos Títulos das Colunas

            // --- Barra Superior (Cedente / Requerente) ---
            g.FillRectangle(Brushes.WhiteSmoke, x, currentY, pageWidth, hRowHeaderTop); // Fundo bem leve
            g.DrawRectangle(penBorda, x, currentY, pageWidth, hRowHeaderTop);

            // Linha divisória entre Cedente e Requerente
            // O Cedente vai até o final da coluna "Condição Item no Empréstimo" (c7)
            int widthCedente = c1 + c2 + c3 + c4 + c5 + c6 + c7;
            g.DrawLine(penBorda, x + widthCedente, currentY, x + widthCedente, currentY + hRowHeaderTop);

            g.DrawString("CEDENTE", _fontTabelaHeader, brushTexto, new Rectangle(x, currentY, widthCedente, hRowHeaderTop), sfCenter);
            g.DrawString("REQUERENTE", _fontTabelaHeader, brushTexto, new Rectangle(x + widthCedente, currentY, c8, hRowHeaderTop), sfCenter);

            currentY += hRowHeaderTop;

            // --- Cabeçalhos das Colunas ---
            g.FillRectangle(Brushes.WhiteSmoke, x, currentY, pageWidth, hRowHeader);
            g.DrawRectangle(penBorda, x, currentY, pageWidth, hRowHeader);

            int cx = x;
            // Desenha linhas verticais
            DrawVLine(g, penBorda, cx += c1, currentY, hRowHeader);
            DrawVLine(g, penBorda, cx += c2, currentY, hRowHeader);
            DrawVLine(g, penBorda, cx += c3, currentY, hRowHeader);
            DrawVLine(g, penBorda, cx += c4, currentY, hRowHeader);
            DrawVLine(g, penBorda, cx += c5, currentY, hRowHeader);
            DrawVLine(g, penBorda, cx += c6, currentY, hRowHeader);
            DrawVLine(g, penBorda, cx += c7, currentY, hRowHeader);

            // Textos do Header
            cx = x;
            g.DrawString("ITEM", _fontTabelaHeader, brushTexto, new Rectangle(cx, currentY, c1, hRowHeader), sfCenter);
            g.DrawString("NOME FERRAMENTA/EQUIPAMENTO", _fontTabelaHeader, brushTexto, new Rectangle(cx += c1, currentY, c2, hRowHeader), sfCenter);
            g.DrawString("ID VOAR", _fontTabelaHeader, brushTexto, new Rectangle(cx += c2, currentY, c3, hRowHeader), sfCenter);
            g.DrawString("PN", _fontTabelaHeader, brushTexto, new Rectangle(cx += c3, currentY, c4, hRowHeader), sfCenter);
            g.DrawString("SN", _fontTabelaHeader, brushTexto, new Rectangle(cx += c4, currentY, c5, hRowHeader), sfCenter);
            g.DrawString("VALIDADE\nCALIBRAÇÃO", _fontTabelaHeader, brushTexto, new Rectangle(cx += c5, currentY, c6, hRowHeader), sfCenter);
            g.DrawString("CONDIÇÃO\nITEM NO\nEMPRÉSTIMO", _fontTabelaHeader, brushTexto, new Rectangle(cx += c6, currentY, c7, hRowHeader), sfCenter);
            g.DrawString("CONDIÇÃO DO\nITEM NO\nEMPRÉSTIMO", _fontTabelaHeader, brushTexto, new Rectangle(cx += c7, currentY, c8, hRowHeader), sfCenter);

            currentY += hRowHeader;

            // --- Dados da Tabela ---
            int hRow = 25;
            int count = 1;
            StringFormat sfLeft = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };

            foreach (var row in _ferramentas)
            {
                string nome, id, pn, sn, val;

                // Lógica de extração de dados (mantida do seu código original)
                if (row.DataGridView != null && row.DataGridView.Name == "dgvDados")
                {
                    nome = row.Cells["colInstrumento"].Value?.ToString() ?? "";
                    id = row.Cells["colIdentifSOD"].Value?.ToString() ?? "";
                    pn = row.Cells["colPN"].Value?.ToString() ?? "";
                    sn = row.Cells["colSN"].Value?.ToString() ?? "";
                    val = row.Cells["colDataVencimento"].Value?.ToString() ?? "";
                    // Formata data curta se possível
                    if (DateTime.TryParse(val, out DateTime dtVal)) val = dtVal.ToString("dd/MM/yyyy");
                }
                else
                {
                    nome = row.Cells["colSemDescricao"].Value?.ToString() ?? "";
                    id = row.Cells["colSemCodigo"].Value?.ToString() ?? "";
                    pn = row.Cells["colSemPN"].Value?.ToString() ?? "";
                    sn = "-";
                    val = "-";
                }

                // Desenha a linha
                g.DrawRectangle(penBorda, x, currentY, pageWidth, hRow);

                cx = x;
                DrawVLine(g, penBorda, cx += c1, currentY, hRow);
                DrawVLine(g, penBorda, cx += c2, currentY, hRow);
                DrawVLine(g, penBorda, cx += c3, currentY, hRow);
                DrawVLine(g, penBorda, cx += c4, currentY, hRow);
                DrawVLine(g, penBorda, cx += c5, currentY, hRow);
                DrawVLine(g, penBorda, cx += c6, currentY, hRow);
                DrawVLine(g, penBorda, cx += c7, currentY, hRow);

                // Desenha os Textos
                cx = x;
                g.DrawString(count.ToString(), _fontTabelaDados, brushTexto, new Rectangle(cx, currentY, c1, hRow), sfCenter);
                // Nome alinhado à esquerda com margem pequena
                g.DrawString(nome, _fontTabelaDados, brushTexto, new Rectangle(cx += c1 + 4, currentY, c2 - 8, hRow), sfLeft);
                g.DrawString(id, _fontTabelaDados, brushTexto, new Rectangle(cx += c2 - 4, currentY, c3, hRow), sfCenter); // Ajuste do recuo
                g.DrawString(pn, _fontTabelaDados, brushTexto, new Rectangle(cx += c3, currentY, c4, hRow), sfCenter);
                g.DrawString(sn, _fontTabelaDados, brushTexto, new Rectangle(cx += c4, currentY, c5, hRow), sfCenter);
                g.DrawString(val, _fontTabelaDados, brushTexto, new Rectangle(cx += c5, currentY, c6, hRow), sfCenter);

                currentY += hRow;
                count++;
            }

            // ==========================================================
            // 3. OBSERVAÇÕES
            // ==========================================================
            currentY += 15;
            g.DrawString("OBSERVAÇÕES:", _fontTabelaHeader, brushTexto, x, currentY);
            currentY += 15;
            string obsTexto = string.IsNullOrEmpty(_observacao) ? "N/A" : _observacao;
            g.DrawString(obsTexto, _fontTabelaDados, brushTexto, new RectangleF(x, currentY, pageWidth, 30));
            currentY += 40;

            // ==========================================================
            // 4. RODAPÉ / ASSINATURAS
            // ==========================================================
            // Certifica que cabe na página, se não pula
            if (currentY + 150 > e.PageBounds.Height - margin)
            {
                e.HasMorePages = true; // Simplificado, ideal seria criar nova página, mas aqui desenha no final
                return;
            }

            int hSignBox = 140;
            g.DrawRectangle(penBorda, x, currentY, pageWidth, hSignBox);

            // Header do Rodapé
            int hSignHeader = 25;
            g.DrawLine(penBorda, x, currentY + hSignHeader, x + pageWidth, currentY + hSignHeader);
            g.DrawLine(penBorda, x + (pageWidth / 2), currentY, x + (pageWidth / 2), currentY + hSignBox); // Divisória vertical

            g.DrawString("CEDENTE (VOAR)", _fontTabelaHeader, brushTexto, new Rectangle(x, currentY, pageWidth / 2, hSignHeader), sfCenter);
            g.DrawString("REQUERENTE", _fontTabelaHeader, brushTexto, new Rectangle(x + (pageWidth / 2), currentY, pageWidth / 2, hSignHeader), sfCenter);

            // Dados Cedente/Requerente
            int yDados = currentY + hSignHeader + 10;
            int xLeft = x + 10;
            int xRight = x + (pageWidth / 2) + 10;
            int spacing = 18;

            // Lado Esquerdo
            g.DrawString($"NOME: {_cedNome}", _fontTabelaDados, brushTexto, xLeft, yDados);
            g.DrawString($"EMPRESA: {_cedEmpresa}", _fontTabelaDados, brushTexto, xLeft, yDados + spacing);
            g.DrawString($"EMAIL: {_cedEmail}", _fontTabelaDados, brushTexto, xLeft, yDados + spacing * 2);
            g.DrawString($"TEL: {_cedTel}", _fontTabelaDados, brushTexto, xLeft, yDados + spacing * 3);

            // Lado Direito
            g.DrawString($"NOME: {_reqNome}", _fontTabelaDados, brushTexto, xRight, yDados);
            g.DrawString($"EMPRESA: {_reqEmpresa}", _fontTabelaDados, brushTexto, xRight, yDados + spacing);
            g.DrawString($"EMAIL: {_reqEmail}", _fontTabelaDados, brushTexto, xRight, yDados + spacing * 2);
            g.DrawString($"TEL: {_reqTel}", _fontTabelaDados, brushTexto, xRight, yDados + spacing * 3);

            // Linha de Assinatura
            int yAss = currentY + hSignBox - 30;
            int wLine = (pageWidth / 2) - 40;

            // Assinatura Esquerda
            g.DrawLine(penBorda, x + 20, yAss, x + 20 + wLine, yAss);
            g.DrawString("Assinatura / Data", _fontLabel, brushTexto, x + 20, yAss + 5);

            // Assinatura Direita
            g.DrawLine(penBorda, xRight + 10, yAss, xRight + 10 + wLine, yAss);
            g.DrawString("Assinatura / Data", _fontLabel, brushTexto, xRight + 10, yAss + 5);
        }

        // Helper para desenhar célula do cabeçalho com Label cinza e Valor preto
        private void DrawHeaderCell(Graphics g, string label, string valor, int x, int y, int w, int h)
        {
            StringFormat sfLabel = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Near };
            StringFormat sfValue = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Far };

            // Label um pouco menor e cinza escuro para diferenciar
            g.DrawString(label, _fontLabel, Brushes.DimGray, new Rectangle(x, y + 2, w, h), sfLabel);
            // Valor em negrito
            g.DrawString(valor, _fontValor, Brushes.Black, new Rectangle(x, y - 4, w, h), sfValue);
        }

        private void DrawVLine(Graphics g, Pen p, int x, int y, int h)
        {
            g.DrawLine(p, x, y, x, y + h);
        }
    }
}