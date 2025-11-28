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

        // MODELOS
        public class Instrumento
        {
            public int ID { get; set; }
            public string InstrumentoNome { get; set; }
            public string Modelo { get; set; }
            public string PN { get; set; }
            public string SN { get; set; }
            public string IdentifSOD { get; set; }
            public string IdentifOficina { get; set; }
            public string Certificado { get; set; }
            public string DataCalibracao { get; set; }
            public string DataVencimento { get; set; }
            public string Executante { get; set; }
            public string Instalada { get; set; }
            public string Local { get; set; }
            public string SubLocalizacao { get; set; }
            public string Observacoes { get; set; }
            public string Mecanico { get; set; }
            public byte[] Foto { get; set; }
            public byte[] CertificadoPDF { get; set; }
        }
        public class SemCalibracao
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public string Codigo { get; set; }
            public string PN { get; set; }
            public string Fabricante { get; set; }
            public string Local { get; set; }
            public string CadastroLocal { get; set; }
            public string CodLocal { get; set; }
            public string Status { get; set; }
            public string Mecanico { get; set; }
            public byte[] Foto { get; set; }
            public byte[] CertificadoPDF { get; set; }
        }
        public class Mecanico { public string MecanicoID { get; set; } public string Nome { get; set; } public string StatusBloqueio { get; set; } }
        public class MecInfo { public bool Existe { get; set; } public string Nome { get; set; } }
        public class FerrInfo { public bool Encontrada { get; set; } public string MecanicoAtual { get; set; } }

        // --- MÉTODOS COM CALIBRAÇÃO ---
        public static async Task<List<Instrumento>> GetCom()
        {
            var s = await client.GetStringAsync($"{BaseUrl}/api/Ferramentas/ComCalibracao");
            return JsonConvert.DeserializeObject<List<Instrumento>>(s);
        }
        public static async Task PostCom(Instrumento i)
        {
            var c = new StringContent(JsonConvert.SerializeObject(i), Encoding.UTF8, "application/json");
            (await client.PostAsync($"{BaseUrl}/api/Ferramentas/ComCalibracao", c)).EnsureSuccessStatusCode();
        }
        public static async Task PutCom(int id, Instrumento i)
        {
            var c = new StringContent(JsonConvert.SerializeObject(i), Encoding.UTF8, "application/json");
            (await client.PutAsync($"{BaseUrl}/api/Ferramentas/ComCalibracao/{id}", c)).EnsureSuccessStatusCode();
        }
        public static async Task DelCom(int id) => (await client.DeleteAsync($"{BaseUrl}/api/Ferramentas/ComCalibracao/{id}")).EnsureSuccessStatusCode();

        // --- MÉTODOS SEM CALIBRAÇÃO (AGORA COMPLETOS) ---
        public static async Task<List<SemCalibracao>> GetSem()
        {
            var s = await client.GetStringAsync($"{BaseUrl}/api/Ferramentas/SemCalibracao");
            return JsonConvert.DeserializeObject<List<SemCalibracao>>(s);
        }
        public static async Task PostSem(SemCalibracao i)
        {
            var c = new StringContent(JsonConvert.SerializeObject(i), Encoding.UTF8, "application/json");
            (await client.PostAsync($"{BaseUrl}/api/Ferramentas/SemCalibracao", c)).EnsureSuccessStatusCode();
        }
        public static async Task PutSem(int id, SemCalibracao i)
        {
            var c = new StringContent(JsonConvert.SerializeObject(i), Encoding.UTF8, "application/json");
            (await client.PutAsync($"{BaseUrl}/api/Ferramentas/SemCalibracao/{id}", c)).EnsureSuccessStatusCode();
        }
        public static async Task DelSem(int id) => (await client.DeleteAsync($"{BaseUrl}/api/Ferramentas/SemCalibracao/{id}")).EnsureSuccessStatusCode();

        // --- OUTROS MÉTODOS ---
        public static async Task<List<Mecanico>> GetMecs()
        {
            var s = await client.GetStringAsync($"{BaseUrl}/api/Mecanicos");
            return JsonConvert.DeserializeObject<List<Mecanico>>(s);
        }
        public static async Task AddMec(Mecanico m)
        {
            var c = new StringContent(JsonConvert.SerializeObject(m), Encoding.UTF8, "application/json");
            (await client.PostAsync($"{BaseUrl}/api/Mecanicos", c)).EnsureSuccessStatusCode();
        }
        public static async Task DelMec(string id) => (await client.DeleteAsync($"{BaseUrl}/api/Mecanicos/{id}")).EnsureSuccessStatusCode();

        public static async Task<MecInfo> VerMec(string id)
        {
            var s = await client.GetStringAsync($"{BaseUrl}/api/Movimentacao/VerificarMecanico/{id}");
            return JsonConvert.DeserializeObject<MecInfo>(s);
        }
        public static async Task<FerrInfo> VerFerr(string id)
        {
            var s = await client.GetStringAsync($"{BaseUrl}/api/Movimentacao/StatusFerramenta/{id}");
            return JsonConvert.DeserializeObject<FerrInfo>(s);
        }
        public static async Task<string> RegMov(string tipo, string idFerr, string idMec, string nomeMec, string aeronave, string usuarioLogado)
        {
            var d = new
            {
                Tipo = tipo,
                IdFerramenta = idFerr,
                IdMecanico = idMec,
                NomeMecanico = nomeMec,
                Aeronave = aeronave,
                UsuarioLogado = usuarioLogado
            };
            var c = new StringContent(JsonConvert.SerializeObject(d), Encoding.UTF8, "application/json");
            var r = await client.PostAsync($"{BaseUrl}/api/Movimentacao/Registrar", c);
            if (r.IsSuccessStatusCode) return "OK";
            return await r.Content.ReadAsStringAsync();
        }
    }
}