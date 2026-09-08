using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace ValgusfoorApp;

public class ValgusfoorPage : ContentPage
{
    Frame redBox;
    Frame yellowBox;
    Frame greenBox;
    Label statusLabel;

    bool foorSees = false;

    public ValgusfoorPage()
    {
        Title = "Valgusfoor";

        // Pealkiri
        statusLabel = new Label
        {
            Text = "Vali valgus",
            FontSize = 28,
            FontAttributes = FontAttributes.Bold,
            HorizontalOptions = LayoutOptions.Center,
            TextColor = Colors.Black
        };

        // Tuled
        redBox = LooTuli(Colors.Red);
        yellowBox = LooTuli(Colors.Yellow);
        greenBox = LooTuli(Colors.Green);

        // Klõps punasele
        redBox.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(async () =>
            {
                if (!foorSees)
                    return;

                await Vilguta(redBox);
                statusLabel.Text = "Seisa";
            })
        });

        // Klõps kollasele
        yellowBox.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(async () =>
            {
                if (!foorSees)
                    return;

                await Vilguta(yellowBox);
                statusLabel.Text = "Valmista";
            })
        });

        // Klõps rohelisele
        greenBox.GestureRecognizers.Add(new TapGestureRecognizer
        {
            Command = new Command(async () =>
            {
                if (!foorSees)
                    return;

                await Vilguta(greenBox);
                statusLabel.Text = "Sõida";
            })
        });

        // Sisse nupp
        Button sisseButton = new Button
        {
            Text = "Sisse",
            FontSize = 20,
            BackgroundColor = Colors.DarkGreen,
            TextColor = Colors.White,
            CornerRadius = 15
        };

        sisseButton.Clicked += (sender, e) =>
        {
            foorSees = true;

            redBox.BackgroundColor = Colors.Red;
            yellowBox.BackgroundColor = Colors.Yellow;
            greenBox.BackgroundColor = Colors.Green;

            statusLabel.Text = "Vali valgus";
        };

        // Välja nupp
        Button valjaButton = new Button
        {
            Text = "Välja",
            FontSize = 20,
            BackgroundColor = Colors.DarkRed,
            TextColor = Colors.White,
            CornerRadius = 15
        };

        valjaButton.Clicked += (sender, e) =>
        {
            foorSees = false;

            redBox.BackgroundColor = Colors.Gray;
            yellowBox.BackgroundColor = Colors.Gray;
            greenBox.BackgroundColor = Colors.Gray;

            statusLabel.Text = "Lülita esmalt foor sisse";
        };

        // Nuppude paigutus
        HorizontalStackLayout buttons = new HorizontalStackLayout
        {
            Spacing = 20,
            HorizontalOptions = LayoutOptions.Center
        };

        buttons.Children.Add(sisseButton);
        buttons.Children.Add(valjaButton);

        // Kõik elemendid vertikaalselt
        VerticalStackLayout layout = new VerticalStackLayout
        {
            Spacing = 20,
            Padding = 30,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        layout.Children.Add(statusLabel);
        layout.Children.Add(redBox);
        layout.Children.Add(yellowBox);
        layout.Children.Add(greenBox);
        layout.Children.Add(buttons);

        Content = layout;
    }

    // Loob ühe valgusfoori tule
    Frame LooTuli(Color color)
    {
        return new Frame
        {
            WidthRequest = 120,
            HeightRequest = 120,

            // Frame ringiks
            CornerRadius = 60,

            BackgroundColor = Colors.Gray,

            // Eemaldame varju ja ääre
            BorderColor = Colors.DarkGray,
            HasShadow = false,

            HorizontalOptions = LayoutOptions.Center
        };
    }

    // Väike animatsioon tule klõpsamisel
    async Task Vilguta(Frame tuli)
    {
        await Task.WhenAll(
            tuli.ScaleTo(1.2, 150),
            tuli.FadeTo(0.5, 150)
        );

        await Task.WhenAll(
            tuli.ScaleTo(1.0, 150),
            tuli.FadeTo(1.0, 150)
        );
    }
}
