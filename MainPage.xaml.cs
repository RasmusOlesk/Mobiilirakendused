namespace TARge_25_Maui_Olesk
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object? sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";


            var random = new Random();
            var randomColor = Color.FromRgb(
                random.Next(0, 256), // Red
                random.Next(0, 256), // Green
                random.Next(0, 256)  // Blue
            );

            if (count >= 10)
            {
                dotnetbot.IsVisible = false; // Peidab pildi
                CounterBtn.Text = "Pilt kadus ära! Vajuta Reset.";
            }


            if (count >= 5)
            {
                CounterBtn.BackgroundColor = Colors.Red;
                CounterBtn.TextColor = Colors.White;
            }

            // Rakendame värvi teisele nupule või taustale
            ResetBtn.BackgroundColor = randomColor;

            SemanticScreenReader.Announce(CounterBtn.Text);
            ResetBtn.Text = $"Tagasi nulli";
            dotnetbot.Rotation += 10;

            //Pilt muutub suuremaks iga vajutusega
            dotnetbot.Scale += 0.1;

            //Pilt muutub aina nähtavamaks iga vajutusega
            dotnetbot.Opacity += 0.1;
        }

        private void OnResetClicked(object? sender, EventArgs e)
        {
            count = 0;
            CounterBtn.Text = "Vajuta mind";
            CounterBtn.Text = "Alustame uuesti!";
            dotnetbot.Rotation = 0; // Pilt läheb otseks tagasi

            //Pilt läheb tagasi oma originaal suuruseks
            dotnetbot.Scale -= 1;

            //Pilt läheb tagasi oma originaal nähtavuseks
            dotnetbot.Opacity -= 1;

            dotnetbot.IsVisible = true;
            ResetBtn.ClearValue(BackgroundColorProperty);// eemaldab reset nupu taustavärvi
            CounterBtn.ClearValue(BackgroundColorProperty);// eemaldab counter nupu taustavärvi
            CounterBtn.ClearValue(Button.TextColorProperty);// eemaldab counter nupu tekstivärvi
                                                            // Liigutame pildi paremasse serva
         

            // VÕI teeme loogika: kui on vasakul, liiguta paremale, ja vastupidi
            if (dotnetbot.HorizontalOptions == LayoutOptions.Start)
            {
                dotnetbot.HorizontalOptions = LayoutOptions.End;
            }
            else
            {
                dotnetbot.HorizontalOptions = LayoutOptions.Start;
            }
        }

    }
}
