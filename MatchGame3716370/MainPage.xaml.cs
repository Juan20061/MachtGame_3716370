using System.Diagnostics;


namespace MatchGame3716370
{
    public partial class MainPage : ContentPage
    {

        private Button ultimoButtonClicked;
        private bool encontrandoMatch = false;
        private int totalMatches = 0;
        private const int maxMatches = 8;  
        private Stopwatch stopwatch;
        private bool isGameActive = false;

        // Constructor de la clase y aqui se ejecuta el temporizador y la configuracion del juego
        public MainPage()
        {
            InitializeComponent();
            SetUpGame();
            StartTimer();
        }

        // Función para configurar el juego cambiando los emojis de animales en los botones

        private void SetUpGame()
        {
            //lista de los emojis que se mostrara en el juego
            List<string> animalEmoji = new List<string>()
            {
                "🙊", "🙊",
                "🐔", "🐔",
                "🐹", "🐹",
                "🐰", "🐰",
                "🐳", "🐳",
                "🐍", "🐍",
                "🐢", "🐢",
                "🦔", "🦔"
            };

            Random random = new Random();
            foreach (Button button in Grid1.Children.OfType<Button>())
            {
               // Selecciona un emoji al azar y lo asigna al boton
                int index = random.Next(animalEmoji.Count);
                string nextEmoji = animalEmoji[index];
                button.Text = nextEmoji;
                button.IsVisible = true;  // Asegura de que el botn esté visible al reiniciar el juego
                animalEmoji.RemoveAt(index);//quita el emoji seleccionado
            }

            totalMatches = 0;
            // Oculta el botón de reinicio cuando se configura el juego
            RestartButton.IsVisible = false;
            // Inicia el contaodr para medir el tiempo del juego.
            stopwatch = new Stopwatch();
            stopwatch.Start();
            isGameActive = true;
        }
       // Método para manejar los clics en los botones del juego.
        private async void Button_Clicked(object sender, EventArgs e)
        {
            // Verifica si el juego está activo.
            if (!isGameActive)
                return;

            Button button = sender as Button;
            if (button == null || button.IsVisible == false)
                return;

            button.IsVisible = true;

            if (encontrandoMatch == false)
            {
                ultimoButtonClicked = button;
                encontrandoMatch = true;
            }
            else
            { // Compara el texto del botón con el último botón presionado para verificar si hay una coincidencia.
                if (button.Text == ultimoButtonClicked.Text)
                {
                    // Si hay una coincidencia oculta los botones y cambia el contador

                    button.IsVisible = false;
                    ultimoButtonClicked.IsVisible = false;
                    totalMatches++;

                    // Verifica si se han encontrado todas las coincidencias

                    if (totalMatches == maxMatches)
                    {
                        //para el cronometro y se hace visible el boton de reinicio
                        stopwatch.Stop();   
                        RestartButton.IsVisible = true;  
                        isGameActive = false;
                    }

                    encontrandoMatch = false;
                }
                else
                {                
                    // Si no hay coincidencia, espera medio segundo y oculta ambos botones

                    await Task.Delay(500);
                    button.IsVisible = false;
                    ultimoButtonClicked.IsVisible = false;
                    encontrandoMatch = false;
                }
            }
        }
      // metodo para manejar el clic del botón de reinicio
        private void RestartButton_Clicked(object sender, EventArgs e)
        {
            SetUpGame();
        }
        // Método para iniciar y mostrar el temporizador que muestra el tiempo

        private async void StartTimer()
        {
            while (true)
            {
                if (isGameActive)
                {
                    TimeSpan ts = stopwatch.Elapsed;
                    TimerLabel.Text = $"Time: {ts.Minutes:00}:{ts.Seconds:00}";
                }
                await Task.Delay(1000);
            }
        }
    }

}
