using System.Diagnostics;
using System.Security.Claims;

namespace CodeNet.Api.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _proximo; //proximo midd da pipeline
        private readonly ILogger<RequestLoggingMiddleware> _logger; //para os logs

        public RequestLoggingMiddleware(RequestDelegate proximo, ILogger<RequestLoggingMiddleware> logger)
        {
            _proximo = proximo;
            _logger = logger;
        }

        //task padrão para processar requsições
        public async Task InvokeAsync(HttpContext context)
        {
            //começa a medir o tempo
            var stopwatch = Stopwatch.StartNew();

            //pegar qual o método da requisição
            var metodo = context.Request.Method;

            //pegar o caminho solicidato
            var caminho = context.Request.Path;

            //chama o proximo midd ou controller
            await _proximo(context);

            //para o tempo
            stopwatch.Stop();

            //escrever no log as infos
            _logger.LogInformation($"[{metodo}] {caminho} - {stopwatch.ElapsedMilliseconds}ms");
        }
    }
}
