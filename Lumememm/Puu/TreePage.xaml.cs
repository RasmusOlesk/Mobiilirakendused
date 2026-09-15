

namespace Tree;

public partial class TreePage : ContentPage
{
    // Animatsiooni kestus millisekundites
    private uint animationSpeed = 1000;

    public TreePage()
    {
        InitializeComponent();

        // Algväärtused
        VirtualDate.Date = DateTime.Today;
        VirtualTime.Time = DateTime.Now.TimeOfDay;

        ActionPicker.SelectedIndex = 0;
    }

    private async void OnRunClicked(object sender, EventArgs e)
    {
        if (ActionPicker.SelectedIndex == -1)
        {
            InfoLabel.Text = "⚠️ Palun vali tegevus!";
            return;
        }

        string action = ActionPicker.SelectedItem.ToString();

        switch (action)
        {
            case "Kasva":
                await GrowTree();
                break;

            case "Õitse":
                await BloomTree();
                break;

            case "Värise":
                await ShakeTree();
                break;

            case "Langeta":
                await CutTree();
                break;
        }
    }

    // ============================
    // KASVA
    // ============================

    private async Task GrowTree()
    {
        InfoLabel.Text = "🌱 Puu kasvab...";

        // Juhuslik kasvutegur 1.2 kuni 2.0
        Random random = new Random();
        double scale = 1.2 + random.NextDouble() * 0.8;

        await Task.WhenAll(
            Trunk.ScaleTo(scale, animationSpeed),
            Leaves.ScaleTo(scale, animationSpeed)
        );

        InfoLabel.Text = $"🌳 Puu kasvas {scale:F2} korda suuremaks!";
    }

    // ============================
    // ÕITSE
    // ============================

    private async Task BloomTree()
    {
        InfoLabel.Text = "🌸 Puu õitseb!";

        Flower1.IsVisible = true;
        Flower2.IsVisible = true;
        Flower3.IsVisible = true;

        Flower1.Opacity = 0;
        Flower2.Opacity = 0;
        Flower3.Opacity = 0;

        await Task.WhenAll(
            Flower1.FadeTo(1, animationSpeed),
            Flower2.FadeTo(1, animationSpeed),
            Flower3.FadeTo(1, animationSpeed)
        );
    }

    // ============================
    // VÄRISE
    // ============================

    private async Task ShakeTree()
    {
        InfoLabel.Text = "🍃 Puu väriseb tuules...";

        double originalX = TreeArea.TranslationX;

        for (int i = 0; i < 4; i++)
        {
            await TreeArea.TranslateTo(originalX + 15, 0, animationSpeed / 4);
            await TreeArea.TranslateTo(originalX - 15, 0, animationSpeed / 4);
        }

        await TreeArea.TranslateTo(originalX, 0, animationSpeed / 4);

        InfoLabel.Text = "🍃 Tuul vaibus.";
    }

    // ============================
    // LANGETA
    // ============================

    private async Task CutTree()
    {
        // DatePicker.Date on DateTime?
        if (VirtualDate.Date == null)
        {
            InfoLabel.Text = "❌ Kuupäev puudub!";
            return;
        }

        // Võtame nullable kuupäevast tegeliku DateTime väärtuse
        DateTime date = VirtualDate.Date.Value;

        // TimePicker.Time on TimeSpan?
        TimeSpan time = VirtualTime.Time ?? TimeSpan.Zero;

        // Nüüd date on kindlasti DateTime, seega .Month töötab
        int month = date.Month;

        bool isWinter =
            month == 12 ||
            month == 1 ||
            month == 2;

        bool isDay =
            time >= new TimeSpan(8, 0, 0) &&
            time <= new TimeSpan(17, 0, 0);

        if (!isWinter || !isDay)
        {
            InfoLabel.Text =
                "❌ Puid tohib langetada ainult talvel ja valgel ajal (08:00–17:00)!";

            return;
        }

        InfoLabel.Text = "🪓 Puu langeb...";

        await Task.WhenAll(
            Trunk.RotateTo(90, animationSpeed),
            Leaves.RotateTo(90, animationSpeed)
        );

        InfoLabel.Text = "🌳 Puu on langetatud!";
    }


    // ============================
    // LÄBIPAISTVUS
    // ============================

    private void OnOpacityChanged(object sender, ValueChangedEventArgs e)
    {
        double value = e.NewValue;

        // Slider väärtus otse lehestiku läbipaistvuseks
        Leaves.Opacity = value;

        OpacityLabel.Text = value.ToString("F2");
    }

    // ============================
    // KIIRUSE VÄHENDAMINE
    // ============================

    private void OnMinusClicked(object sender, EventArgs e)
    {
        if (animationSpeed > 500)
        {
            animationSpeed -= 100;
        }

        SpeedLabel.Text = $"{animationSpeed} ms";
    }

    // ============================
    // KIIRUSE SUURENDAMINE
    // ============================

    private void OnPlusClicked(object sender, EventArgs e)
    {
        if (animationSpeed < 2000)
        {
            animationSpeed += 100;
        }

        SpeedLabel.Text = $"{animationSpeed} ms";
    }

    // ============================
    // KUUPÄEVA MUUTMINE
    // ============================

    private void OnDateSelected(object sender, DateChangedEventArgs e)
    {
        if (e.NewDate.HasValue)
        {
            ChangeBackgroundBySeason(e.NewDate.Value.Month);
        }
    }


    // ============================
    // DÜNAAMILINE TAEVAS
    // LOOV LISAPUNKT
    // ============================

    private void ChangeBackgroundBySeason(int month)
    {
        switch (month)
        {
            // Kevad
            case 3:
            case 4:
            case 5:
                TreeArea.BackgroundColor = Color.FromArgb("#B8F2B8");
                InfoLabel.Text = "🌱 Kevad – loodus ärkab!";
                break;

            // Suvi
            case 6:
            case 7:
            case 8:
                TreeArea.BackgroundColor = Color.FromArgb("#87CEEB");
                InfoLabel.Text = "☀️ Suvi – puu kasvab!";
                break;

            // Sügis
            case 9:
            case 10:
            case 11:
                TreeArea.BackgroundColor = Color.FromArgb("#F4A460");
                InfoLabel.Text = "🍂 Sügis – lehed langevad.";
                break;

            // Talv
            case 12:
            case 1:
            case 2:
                TreeArea.BackgroundColor = Color.FromArgb("#B0C4DE");
                InfoLabel.Text = "❄️ Talv – puu puhkab.";
                break;
        }
    }
}