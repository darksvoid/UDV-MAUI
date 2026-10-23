using System.Text;

namespace FitnessSurvey;

public partial class MainPage : ContentPage
{
    private static readonly DateTime DefaultStartDate = new DateTime(2026, 11, 2);

    public MainPage()
    {
        InitializeComponent();
        UpdateHint();
    }

    private void OnRequiredFieldChanged(object? sender, TextChangedEventArgs e)
    {
        UpdateHint();
    }

    private void OnShowResultClicked(object? sender, EventArgs e)
    {
        var problems = new List<string>();

        if (string.IsNullOrWhiteSpace(NameEntry.Text))
            problems.Add("имя и фамилия");

        if (string.IsNullOrWhiteSpace(PhoneEntry.Text) || PhoneEntry.Text.Trim().Length < 10)
            problems.Add("телефон (не меньше 10 символов)");

        if (DirectionPicker.SelectedIndex < 0)
            problems.Add("направление тренировок");

        if (problems.Count > 0)
        {
            ResultTitleLabel.Text = "Анкета заполнена не полностью";
            ResultLabel.Text = "Не хватает: " + string.Join(", ", problems) + ".";
            ResultBorder.IsVisible = true;
            return;
        }

        var summary = new StringBuilder();
        summary.AppendLine($"Имя и фамилия: {NameEntry.Text!.Trim()}");
        summary.AppendLine($"Телефон: {PhoneEntry.Text!.Trim()}");
        summary.AppendLine($"Направление: {DirectionPicker.SelectedItem}");
        summary.AppendLine($"Уровень подготовки: {GetSelectedLevel()}");
        summary.AppendLine($"Персональный тренер: {YesNo(TrainerCheck.IsChecked)}");
        summary.AppendLine($"Рассылка клуба: {YesNo(NewsCheck.IsChecked)}");
        summary.AppendLine($"Утренние тренировки: {YesNo(MorningSwitch.IsToggled)}");
        summary.AppendLine($"Дата первой тренировки: {StartDatePicker.Date:dd.MM.yyyy}");
        summary.AppendLine($"Тренировок в неделю: {Math.Round(VisitsSlider.Value)}");
        summary.Append($"Комментарий: {GetComment()}");

        ResultTitleLabel.Text = "Анкета принята";
        ResultLabel.Text = summary.ToString();
        ResultBorder.IsVisible = true;
    }

    private void OnClearClicked(object? sender, EventArgs e)
    {
        NameEntry.Text = string.Empty;
        PhoneEntry.Text = string.Empty;
        CommentEditor.Text = string.Empty;

        DirectionPicker.SelectedIndex = -1;
        LevelBeginner.IsChecked = true;
        TrainerCheck.IsChecked = false;
        NewsCheck.IsChecked = true;
        MorningSwitch.IsToggled = false;
        StartDatePicker.Date = DefaultStartDate;
        VisitsSlider.Value = 3;

        ResultTitleLabel.Text = "Ваши ответы";
        ResultLabel.Text = string.Empty;
        ResultBorder.IsVisible = false;

        UpdateHint();
    }

    private void UpdateHint()
    {
        bool filled = !string.IsNullOrWhiteSpace(NameEntry.Text)
                      && !string.IsNullOrWhiteSpace(PhoneEntry.Text);

        HintLabel.IsVisible = !filled;
    }

    private string GetSelectedLevel()
    {
        RadioButton[] buttons = { LevelBeginner, LevelMiddle, LevelAdvanced };

        foreach (RadioButton button in buttons)
        {
            if (button.IsChecked)
                return button.Value?.ToString() ?? "не выбран";
        }

        return "не выбран";
    }

    private string GetComment()
    {
        return string.IsNullOrWhiteSpace(CommentEditor.Text)
            ? "нет"
            : CommentEditor.Text.Trim();
    }

    private static string YesNo(bool value)
    {
        return value ? "да" : "нет";
    }
}
