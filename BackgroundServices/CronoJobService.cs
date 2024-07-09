using Cronos;

namespace ProcesosAutomaticos.Api.BackgroundServices
{
    // Clase abstracta CronoJobService que implementa las interfaces IHostedService y IDisposable
    public abstract class CronoJobService(string cronExpression, TimeZoneInfo timeZoneInfo) : IHostedService, IDisposable
    {
        // Temporizador que se utilizará para programar la ejecución del trabajo
        private System.Timers.Timer _timer;

        // Expresión cron para determinar las ocurrencias del trabajo
        private readonly CronExpression _expression = CronExpression.Parse(cronExpression);

        // Información de la zona horaria para la programación del trabajo
        private readonly TimeZoneInfo _timeZoneInfo = timeZoneInfo;

        // Método que se llama cuando el servicio se inicia
        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            // Programa el trabajo
            await ScheduleJob(cancellationToken);
        }

        // Método protegido que programa el trabajo basado en la expresión cron
        protected virtual async Task ScheduleJob(CancellationToken cancellationToken)
        {
            // Obtiene la siguiente ocurrencia de la expresión cron
            var next = _expression.GetNextOccurrence(DateTimeOffset.Now, _timeZoneInfo);

            // Verifica si hay una próxima ocurrencia
            if (next.HasValue)
            {
                // Calcula el tiempo de retraso hasta la próxima ocurrencia
                var delay = next.Value - DateTimeOffset.Now;

                // Previene valores no positivos que puedan ser pasados al temporizador
                if (delay.TotalMilliseconds <= 0)
                {
                    await ScheduleJob(cancellationToken);  // Reprograma si el retraso es no positivo
                }

                // Configura el temporizador con el tiempo de retraso calculado
                _timer = new System.Timers.Timer(delay.TotalMilliseconds);

                // Define el evento que se dispara cuando el temporizador se completa
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();  // Resetea y libera el temporizador
                    _timer = null;

                    // Ejecuta el trabajo si no se ha solicitado la cancelación
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await DoWork(cancellationToken);
                    }

                    // Reprograma el próximo trabajo si no se ha solicitado la cancelación
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        await ScheduleJob(cancellationToken);
                    }
                };

                // Inicia el temporizador
                _timer.Start();
            }

            // Completa la tarea
            await Task.CompletedTask;
        }

        // Método virtual para realizar el trabajo, debe ser sobrescrito por las clases derivadas
        public virtual async Task DoWork(CancellationToken cancellationToken)
        {
            // Simula la realización del trabajo con un retraso de 5 segundos
            await Task.Delay(5000, cancellationToken);
        }

        // Método que se llama cuando el servicio se detiene
        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            // Detiene el temporizador si está corriendo
            _timer?.Stop();
            await Task.CompletedTask;
        }

        // Método para liberar los recursos del temporizador
        public virtual void Dispose()
        {
            _timer?.Dispose();
        }
    }
}