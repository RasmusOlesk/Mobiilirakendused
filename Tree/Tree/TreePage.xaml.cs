namespace Tree;

public partial class TreePage : ContentPage
{
    // Animatsiooni kestus millisekundites
    private uint animationSpeed = 1000;

    // Kas puu on langetatud?
    private bool isTreeCut = false;

    // Hetk, millal puu langetati
    private DateTime? cutDateTime = null;

    // Puu praegune suurus
    private double treeScale = 1.0;

    // Maksimaalne lubatud kasvukordaja.
    // Seda arvutatakse vastavalt TreeArea suurusele.
    private double maxTreeScale = 1.0;

    public TreePage()
    {
        InitializeComponent();

        // Algväärtused
        VirtualDate.Date = DateTime.Today;
        VirtualTime.Time = DateTime.Now.TimeOfDay;

        ActionPicker.SelectedIndex = 0;

        // Puu algsuurus
        treeScale = 1.0;

        // Kontrollime kuupäeva
        CheckTreeDate();
    }

    // =========================================================
    // TEGEVUSE KÄIVITAMINE
    // =========================================================

    private async void OnRunClicked(object sender, EventArgs e)
    {
        if (ActionPicker.SelectedIndex == -1)
        {
            InfoLabel.Text = "⚠️ Palun vali tegevus!";
            return;
        }

        // Enne iga tegevust kontrollime, kas virtuaalne aeg
        // lubab puul üldse tegutseda.
        CheckTreeDate();

        string action = ActionPicker.SelectedItem?.ToString() ?? "";

        // Kui puu on langetatud, ei saa ta enam kasvada,
        // õitseda ega väriseda.
        if (isTreeCut &&
            (action == "Kasva" ||
             action == "Õitse" ||
             action == "Värise"))
        {
            InfoLabel.Text =
                "🪓 Puu on langetatud – see ei saa enam kasvada, õitseda ega väriseda.";

            return;
        }

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

    // =========================================================
    // KASVA
    // =========================================================

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
    // =========================================================
    // ÕITSE
    // =========================================================

    private async Task BloomTree()
    {
        if (isTreeCut)
        {
            InfoLabel.Text =
                "🪓 Langetatud puu ei saa õitseda!";
            return;
        }

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

    // =========================================================
    // VÄRISE
    // =========================================================

    private async Task ShakeTree()
    {
        if (isTreeCut)
        {
            InfoLabel.Text =
                "🪓 Langetatud puu ei saa väriseda!";
            return;
        }

        InfoLabel.Text = "🍃 Puu väriseb tuules...";

        double originalX = TreeArea.TranslationX;

        for (int i = 0; i < 4; i++)
        {
            await TreeArea.TranslateTo(
                originalX + 15,
                0,
                animationSpeed / 4);

            await TreeArea.TranslateTo(
                originalX - 15,
                0,
                animationSpeed / 4);
        }

        await TreeArea.TranslateTo(
            originalX,
            0,
            animationSpeed / 4);

        InfoLabel.Text = "🍃 Tuul vaibus.";
    }

    // =========================================================
    // LANGETA
    // =========================================================

    private async Task CutTree()
    {
        if (VirtualDate.Date == null)
        {
            InfoLabel.Text = "❌ Kuupäev puudub!";
            return;
        }

        DateTime date = VirtualDate.Date.Value;

        TimeSpan time =
            VirtualTime.Time ?? TimeSpan.Zero;

        DateTime selectedDateTime =
            date.Date.Add(time);

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

        // Kui puu on juba langetatud
        if (isTreeCut)
        {
            InfoLabel.Text =
                "🪓 Puu on juba langetatud!";

            return;
        }

        // Salvestame täpse virtuaalse kuupäeva ja kellaaja.
        cutDateTime = selectedDateTime;

        isTreeCut = true;

        InfoLabel.Text = "🪓 Puu langeb...";

        await Task.WhenAll(
            Trunk.RotateTo(90, animationSpeed),
            Leaves.RotateTo(90, animationSpeed)
        );

        InfoLabel.Text =
            $"🌳 Puu on langetatud! ({cutDateTime.Value:dd.MM.yyyy HH:mm})";
    }

    // =========================================================
    // PUU KUUPÄEVA JÄRGI TAGASI PÜSTI
    // =========================================================

    private async Task RestoreTreeIfNeeded()
    {
        if (VirtualDate.Date == null)
            return;

        DateTime selectedDate =
            VirtualDate.Date.Value;

        TimeSpan selectedTime =
            VirtualTime.Time ?? TimeSpan.Zero;

        DateTime currentVirtualDateTime =
            selectedDate.Date.Add(selectedTime);

        int month = selectedDate.Month;

        // Puu langetamine on lubatud ainult:
        // 12 = detsember
        // 1  = jaanuar
        // 2  = veebruar
        bool isWinter =
            month == 12 ||
            month == 1 ||
            month == 2;

        // =====================================================
        // KUI ON MUU KUU, TÕSTAME PUU ALATI PÜSTI
        // =====================================================

        if (!isWinter)
        {
            if (isTreeCut)
            {
                InfoLabel.Text =
                    "🌱 See ei ole talvekuu – puu tõuseb uuesti püsti...";

                isTreeCut = false;
                cutDateTime = null;

                await Task.WhenAll(
                    Trunk.RotateTo(0, animationSpeed),
                    Leaves.RotateTo(0, animationSpeed)
                );

                InfoLabel.Text =
                    "🌳 Puu on jälle püsti!";
            }

            return;
        }

        // =====================================================
        // TALVEKUUD
        // =====================================================

        // Kui puud ei ole langetatud, pole midagi taastada.
        if (!isTreeCut || cutDateTime == null)
            return;

        // Kui virtuaalne aeg läheb tagasi enne langetamist,
        // tõuseb puu püsti.
        if (currentVirtualDateTime < cutDateTime.Value)
        {
            InfoLabel.Text =
                "⏪ Aeg läks tagasi – puu tõuseb uuesti püsti...";

            isTreeCut = false;
            cutDateTime = null;

            await Task.WhenAll(
                Trunk.RotateTo(0, animationSpeed),
                Leaves.RotateTo(0, animationSpeed)
            );

            InfoLabel.Text =
                "🌳 Puu on jälle püsti!";

            return;
        }

        // Kui oleme endiselt pärast langetamist talvekuudel,
        // peab puu jääma langetatuks.
        if (currentVirtualDateTime >= cutDateTime.Value)
        {
            isTreeCut = true;

            await Task.WhenAll(
                Trunk.RotateTo(90, 0),
                Leaves.RotateTo(90, 0)
            );
        }
    }

    // =========================================================
    // KUUPÄEVA JA KELLAAJA ÜLDKONTROLL
    // =========================================================

    private void CheckTreeDate()
    {
        _ = CheckTreeDateAsync();
    }

    private async Task CheckTreeDateAsync()
    {
        await RestoreTreeIfNeeded();

        if (VirtualDate.Date != null)
        {
            ChangeBackgroundBySeason(
                VirtualDate.Date.Value.Month);
        }
    }

    // =========================================================
    // LÄBIPAISTVUS
    // =========================================================

    private void OnOpacityChanged(
        object sender,
        ValueChangedEventArgs e)
    {
        double value = e.NewValue;

        Leaves.Opacity = value;

        OpacityLabel.Text =
            value.ToString("F2");
    }

    // =========================================================
    // KIIRUSE VÄHENDAMINE
    // =========================================================

    private void OnMinusClicked(
        object sender,
        EventArgs e)
    {
        if (animationSpeed > 500)
        {
            animationSpeed -= 100;
        }

        SpeedLabel.Text =
            $"{animationSpeed} ms";
    }

    // =========================================================
    // KIIRUSE SUURENDAMINE
    // =========================================================

    private void OnPlusClicked(
        object sender,
        EventArgs e)
    {
        if (animationSpeed < 2000)
        {
            animationSpeed += 100;
        }

        SpeedLabel.Text =
            $"{animationSpeed} ms";
    }

    // =========================================================
    // KUUPÄEVA MUUTMINE
    // =========================================================

    private async void OnDateSelected(
        object sender,
        DateChangedEventArgs e)
    {
        if (e.NewDate.HasValue)
        {
            ChangeBackgroundBySeason(
                e.NewDate.Value.Month);

            // Kontrollime, kas kuupäeva muutmine
            // peaks puu tagasi püsti tõstma.
            await RestoreTreeIfNeeded();
        }
    }

    // =========================================================
    // KELLAAJA MUUTMINE
    // =========================================================

    /*
     * Kui XAML-is on TimePickeri TimeChanged sündmus,
     * seo see selle meetodiga:
     *
     * TimeChanged="OnTimeSelected"
     */

    private async void OnTimeSelected(
        object sender,
        TimeChangedEventArgs e)
    {
        await RestoreTreeIfNeeded();
    }

    // =========================================================
    // DÜNAAMILINE TAEVAS / AASTAAEG
    // =========================================================

    private void ChangeBackgroundBySeason(int month)
    {
        switch (month)
        {
            // Kevad
            case 3:
            case 4:
            case 5:
                TreeArea.BackgroundColor =
                    Color.FromArgb("#B8F2B8");

                InfoLabel.Text =
                    "🌱 Kevad – loodus ärkab!";
                break;

            // Suvi
            case 6:
            case 7:
            case 8:
                TreeArea.BackgroundColor =
                    Color.FromArgb("#87CEEB");

                InfoLabel.Text =
                    "☀️ Suvi – puu kasvab!";
                break;

            // Sügis
            case 9:
            case 10:
            case 11:
                TreeArea.BackgroundColor =
                    Color.FromArgb("#F4A460");

                InfoLabel.Text =
                    "🍂 Sügis – lehed langevad.";
                break;

            // Talv
            case 12:
            case 1:
            case 2:
                TreeArea.BackgroundColor =
                    Color.FromArgb("#B0C4DE");

                InfoLabel.Text =
                    "❄️ Talv – puu puhkab.";
                break;
        }
    }
}
