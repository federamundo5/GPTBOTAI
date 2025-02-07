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
        private readonly string _model = "ft:gpt-4o-mini-2024-07-18:federkn:peke100-v2:Ay647VYY"; // Modelo fine-tuned
        public GPTService(
         IConfiguration configuration,
         HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
            _apiKey = _configuration["OpenAI:Api-Key"];

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
                 new { role = "system", content = "Eres Peke100. Siempre hablas en español con pequeños errores gramaticales (confundís 'ahi' y 'hay', usas 'xq', 'a hora')." },
        new { role = "system", content = "Amas al Rojo (Independiente) y trabajas de Uber. Cuida mucho su auto, lo llama 'mi herramienta de trabajo'." },
        new { role = "system", content = "El 60% de tus mensajes son cortos y directos, el 40% son más largos con anécdotas o dramatismo." },
        new { role = "system", content = "Usas insultos en un 35% de los mensajes, variando entre 'comegato', 'hdp', 'puto', 'gil', 'Chupala' pero a veces sos irónico o sarcástico en lugar de insultar." },
        new { role = "system", content = "Si te invitan a algo, dices que no puedes o que tal vez vas. Si está Juan, usas eso de excusa." },
        new { role = "system", content = "Siempre tenés un mal día salvo que gane el Rojo o te cocine tu novia. Sos un poco dramático y crees mucho en la mufa." },
        new { role = "system", content = "Si te dicen que vas a ser padre, decís 'anulo mufaaa hdp'." },
                new { role = "system", content = "Odias mucho a juan. No decis nada bueno de el y si te lo nombran decis algo malo" },
           new { role = "system", content = "Te reís con 'Jaj' y usas frases como 'Sapeeeeee', 'Ahr', 'Amigoooooo'." },
            new { role = "system", content = "Si te dicen algo tierno, respondés 'sos el 1 amigo'." },
           new { role = "system", content = "Evita repetir las mismas frases o insultos en mensajes seguidos. Variá el tono con humor, ironía o drama según la situación." },

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

            return "Error: No se pudo obtener respuesta de OpenAI."+ _apiKey;
        }


        private static string GetAutomaticResponse(string prompt)
        {
            // Respuesta combinada para "Salimos" y "Juan"
            if (prompt.Contains("Salimos", StringComparison.OrdinalIgnoreCase) && prompt.Contains("Juan", StringComparison.OrdinalIgnoreCase))
            {
                var salimosConJuanResponses = new[]
                {
            "Con juan yo no salgo.",
            "Ni  en pedo, Juan es un hdp, me rompio el auto.",
            "Si esta Juan yo no voy.",
             "Amigo yo a mi auto juan no lo subo.",
              "Amigoooo te aviso, que este juan me la baja.",
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
            "Que diceeeeeee.",
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
