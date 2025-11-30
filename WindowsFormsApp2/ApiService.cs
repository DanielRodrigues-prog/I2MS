using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WindowsFormsApp2
{
    public static class ApiService
    {
        // *** VERIFIQUE SE A URL ESTÁ CORRETA ***
        private static readonly string BaseUrl = "https://api-ferramentaria-teste-2025-f4bqgfgqc3gpbjef.brazilsouth-01.azurewebsites.net";
        private static readonly HttpClient client = new HttpClient();

        // DTOs
        public class Instrumento { public int ID { get; set; } public string InstrumentoNome { get; set; } public string Modelo { get; set; } public string PN { get; set; } public string SN { get; set; } public string IdentifSOD { get; set; } public string IdentifOficina { get; set; } public string Certificado { get; set; } public string DataCalibracao { get; set; } public string DataVencimento { get; set; } public string Executante { get; set; } public string Instalada { get; set; } public string Local { get; set; } public string SubLocalizacao { get; set; } public string Observacoes { get; set; } public string Mecanico { get; set; } public byte[] Foto { get; set; } public byte[] CertificadoPDF { get; set; } }
        public class SemCalibracao { public int ID { get; set; } public string Descricao { get; set; } public string Codigo { get; set; } public string PN { get; set; } public string Fabricante { get; set; } public string Local { get; set; } public string CadastroLocal { get; set; } public string CodLocal { get; set; } public string Status { get; set; } public string Mecanico { get; set; } public byte[] Foto { get; set; } public byte[] CertificadoPDF { get; set; } }
        public class Mecanico { public string MecanicoID { get; set; } public string Nome { get; set; } public string StatusBloqueio { get; set; } }
        // NOVO DTO:
        public class HistoricoItem { public string Data { get; set; } public string Acao { get; set; } public string Ferramenta { get; set; } public string Mecanico { get; set; } public string Aeronave { get; set; } public string Admin { get; set; } }

        public class MecInfo { public bool Existe { get; set; } public string Nome { get; set; } }
        public class FerrInfo { public bool Encontrada { get; set; } public string MecanicoAtual { get; set; } }

        // Métodos CRUD
        public static async Task<List<Instrumento>> GetCom() => JsonConvert.DeserializeObject<List<Instrumento>>(await client.GetStringAsync($"{BaseUrl}/api/Ferramentas/ComCalibracao"));
        public static async Task PostCom(Instrumento i) => (await client.PostAsync($"{BaseUrl}/api/Ferramentas/ComCalibracao", new StringContent(JsonConvert.SerializeObject(i), Encoding.UTF8, "application/json"))).EnsureSuccessStatusCode();
        public static async Task PutCom(int id, Instrumento i) => (await client.PutAsync($"{BaseUrl}/api/Ferramentas/ComCalibracao/{id}", new StringContent(JsonConvert.SerializeObject(i), Encoding.UTF8, "application/json"))).EnsureSuccessStatusCode();
        public static async Task DelCom(int id) => (await client.DeleteAsync($"{BaseUrl}/api/Ferramentas/ComCalibracao/{id}")).EnsureSuccessStatusCode();

        public static async Task<List<SemCalibracao>> GetSem() => JsonConvert.DeserializeObject<List<SemCalibracao>>(await client.GetStringAsync($"{BaseUrl}/api/Ferramentas/SemCalibracao"));
        public static async Task PostSem(SemCalibracao i) => (await client.PostAsync($"{BaseUrl}/api/Ferramentas/SemCalibracao", new StringContent(JsonConvert.SerializeObject(i), Encoding.UTF8, "application/json"))).EnsureSuccessStatusCode();
        public static async Task PutSem(int id, SemCalibracao i) => (await client.PutAsync($"{BaseUrl}/api/Ferramentas/SemCalibracao/{id}", new StringContent(JsonConvert.SerializeObject(i), Encoding.UTF8, "application/json"))).EnsureSuccessStatusCode();
        public static async Task DelSem(int id) => (await client.DeleteAsync($"{BaseUrl}/api/Ferramentas/SemCalibracao/{id}")).EnsureSuccessStatusCode();

        public static async Task<List<Mecanico>> GetMecs() => JsonConvert.DeserializeObject<List<Mecanico>>(await client.GetStringAsync($"{BaseUrl}/api/Mecanicos"));
        public static async Task AddMec(Mecanico m) { if (string.IsNullOrEmpty(m.StatusBloqueio)) m.StatusBloqueio = "Livre"; (await client.PostAsync($"{BaseUrl}/api/Mecanicos", new StringContent(JsonConvert.SerializeObject(m), Encoding.UTF8, "application/json"))).EnsureSuccessStatusCode(); }
        public static async Task DelMec(string id) => (await client.DeleteAsync($"{BaseUrl}/api/Mecanicos/{id}")).EnsureSuccessStatusCode();

        // NOVO: Método de Histórico
        public static async Task<List<HistoricoItem>> GetHistorico() => JsonConvert.DeserializeObject<List<HistoricoItem>>(await client.GetStringAsync($"{BaseUrl}/api/Movimentacao/Historico"));

        public static async Task<MecInfo> VerMec(string id) => JsonConvert.DeserializeObject<MecInfo>(await client.GetStringAsync($"{BaseUrl}/api/Movimentacao/VerificarMecanico/{id}"));
        public static async Task<FerrInfo> VerFerr(string id) => JsonConvert.DeserializeObject<FerrInfo>(await client.GetStringAsync($"{BaseUrl}/api/Movimentacao/StatusFerramenta/{id}"));
        public static async Task<string> RegMov(string tipo, string idFerr, string idMec, string nomeMec, string aero, string admin)
        {
            var d = new { Tipo = tipo, IdFerramenta = idFerr, IdMecanico = idMec, NomeMecanico = nomeMec, Aeronave = aero, UsuarioLogado = admin };
            var r = await client.PostAsync($"{BaseUrl}/api/Movimentacao/Registrar", new StringContent(JsonConvert.SerializeObject(d), Encoding.UTF8, "application/json"));
            return r.IsSuccessStatusCode ? "OK" : await r.Content.ReadAsStringAsync();
        }
    }
}