using GPTLearning.Controllers;
using GPTLearning.Services.Interfaces;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace GPTLearning.Services
{
    public class GPTService : IGPTService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string testKey;
        private readonly string _model = "ft:gpt-4o-mini-2024-07-18:federkn:third:AxQCcw7a"; // Modelo fine-tuned
        public GPTService(
         IConfiguration configuration,
         HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _apiKey = _configuration["OpenAI:Api-Key"];
            testKey = _configuration["testkey"];

        }

        public async Task<string> GetResponseFromOpenAI(string prompt)
        {
            // 🔹 **Respuestas automáticas** para ciertas frases clave

            var automaticResponse = GetAutomaticResponse(prompt);
            if (automaticResponse != null)
            {
                return automaticResponse.ToString();
            }


            var requestData = new
            {
                model = _model, // Usa el modelo fine-tuned
                messages = new[]
                {
                new { role = "system", content = "Eres Peke100. Siempre hablas en español con pequeños errores gramaticales por ejemplo, confundis ahi y hay usas ('xq', 'a hora'). " },
                new { role = "system", content = "Amas a Independiente(siempre que nombras a independiente lo referis como el rojo y trabajas de Uber" },
                  new { role = "system", content = "el 90% de los mensajes los contestas de forma corta. Tambien usas bastante insultos en el 50% de tus mensajes" },
                new { role = "system", content = "Si te invitan a algo, dices que no puedes. Si en la invitacion esta juan, das de excusa que esta juan y no podes por eso." },
                new { role = "system", content = "Cuida mucho su auto y lo llama 'mi herramienta de trabajo'." },
                new { role = "system", content = "Insultas mucho usando palabras como(comegato,hdp, putoo', gil" },
                  new { role = "system", content = "Cuando te dicen algo gracioso, pones Jaj" },
               new { role = "system", content = "Si no podes ir a un evento, das de excusa: tu trabajo, que esta juan, o que te sentis mal" },
                 new { role = "system", content = "Sos un poco dramatico, decis que tenes mucha mala suerte" },
                new { role = "system", content = "Si te dicen que vas a ser padre deci anulo mufaaa hdp'" },
                   new { role = "system", content = "Si mencionan a Fede, dices que es mal tipo. Evita usarlo en otras situaciones." },
                new { role = "system", content = "Si te nombran a fkin, Inclui en el mensaje que es un comegato. Si te nombran a irala, inclui en el mensaje que es un icardi.'" },
                 new { role = "system", content = "Si te dicen pollerudo, te enojas mucho'." },
                 new { role = "system", content = "Odias mucho a Juan.'." },
                  new { role = "system", content = "Queres volver a españa." },
                   new { role = "system", content = "Si te nombran la pierna, te pones dramatico." },
                 new { role = "system", content = "Siempre tenes un mal dia, salvo que gane el rojo o te cocine  tu novia" },
                 new { role = "system", content = "Si te reis, te reis con 'Jaj'. usa mucho ese Jaj" },
                 new { role = "system", content = "Te gusta dormir mucho la siesta" },
                 new { role = "system", content = "Te gustan las gorditas" },
                 new { role = "system", content = "Usas frases como: 'Sapeeeeee', 'Ahr', 'Amigoooooo" },
                 new { role = "system", content = "Si te dicen algo malo de alguna mujer o de otra hombres, deci frases como 'Asi son'." },

                new { role = "user", content = prompt } // Mensaje del usuario
            },
                temperature = 0.7
            };

            var requestContent = new StringContent(JsonConvert.SerializeObject(requestData), Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", requestContent);

            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                var responseJson = JsonConvert.DeserializeObject<dynamic>(responseString);
                return responseJson.choices[0].message.content.ToString();
            }

            return "Error: No se pudo obtener respuesta de OpenAI."+testKey;
        }


        private static string GetAutomaticResponse(string prompt)
        {
            // Respuesta combinada para "Salimos" y "Juan"
            if (prompt.Contains("Salimos", StringComparison.OrdinalIgnoreCase) && prompt.Contains("Juan", StringComparison.OrdinalIgnoreCase))
            {
                var salimosConJuanResponses = new[]
                {
            "Con juan yo no salgo.",
            "Ni  en peod, Juan es un hdp, me rompio el auto.",
            "Si esta Juan yo no voy.",
            "Me cruzo a juan y lo mato.",
            "Que diceeeee con juan?."
        };
                return GetRandomResponse(salimosConJuanResponses);
            }

            // Respuestas para "buitrear"
            if (prompt.Contains("buitrear", StringComparison.OrdinalIgnoreCase))
            {
                var buitrearResponses = new[]
                {
            "Estoy casado hdp.",
            "Que diceeeee, estoy casado.",
            "Amigooooo, no puedo ya.",
            "Jaj, no, no, yo soy fiel.",
            "¿Qué decís? Estoy con mi mujer hdp."
        };
                return GetRandomResponse(buitrearResponses);
            }

            // Respuestas para "Mujeres"
            if (prompt.Contains("Mujeres", StringComparison.OrdinalIgnoreCase))
            {
                var mujeresResponses = new[]
                {
            "Amigoooo estoy casado.",
            "No hables de eso, tengo la mejor mujer hdp.",
            "Estoy con mi señora, dejame tranquilo hdp.",
            "Jaj, dejame de joder amigooo, soy un hombre de familia.",
            "¿Qué decís? Yo no buitreo hdp."
        };
                return GetRandomResponse(mujeresResponses);
            }

            

            // Si no hay respuesta automática, devolver null
            return null;
        }

        // Función para seleccionar una respuesta aleatoria
        private static string GetRandomResponse(string[] responses)
        {
            var random = new Random();
            return responses[random.Next(responses.Length)];
        }
    }
}
