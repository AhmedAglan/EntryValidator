using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EntryValidator.Behaviors.Base;
using Xamarin.Forms;

namespace EntryValidator.Behaviors
{
  public class EntryValidatorBehavior : BehaviorBase<Entry>
  {
    #region IsValidProperty
    public static readonly BindableProperty IsValidProperty =
  BindableProperty.Create(nameof(IsValid), typeof(bool),
    typeof(EntryValidatorBehavior), false, BindingMode.OneWayToSource);

    public bool IsValid
    {
      get { return (bool)GetValue(IsValidProperty); }
      set { SetValue(IsValidProperty, value); }
    }
    #endregion

    #region IsRequiredProperty

    public static readonly BindableProperty IsRequiredProperty =
  BindableProperty.Create(nameof(IsRequired), typeof(bool),
    typeof(EntryValidatorBehavior), true, BindingMode.OneWayToSource);

    public bool IsRequired
    {
      get { return (bool)GetValue(IsRequiredProperty); }
      set { SetValue(IsRequiredProperty, value); }
    }
    #endregion

    #region LengthProperty
    public static readonly BindableProperty LengthProperty =
      BindableProperty.Create(nameof(Length), typeof(int),
        typeof(EntryValidatorBehavior), 0, BindingMode.TwoWay);

    public int Length
    {
      get { return (int)GetValue(LengthProperty); }
      set { SetValue(LengthProperty, value); }
    }
    #endregion

    #region IsEmailProperty
    const string emailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
@"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

    public static readonly BindableProperty IsEmailProperty =
  BindableProperty.Create(nameof(IsEmail), typeof(bool),
    typeof(EntryValidatorBehavior), false, BindingMode.OneWayToSource);

    public bool IsEmail
    {
      get { return (bool)GetValue(IsEmailProperty); }
      set { SetValue(IsEmailProperty, value); }
    }
    #endregion

    #region IsNumberProperty
    public static readonly BindableProperty IsNumberProperty =
  BindableProperty.Create(nameof(IsNumber), typeof(bool),
    typeof(EntryValidatorBehavior), false, BindingMode.OneWayToSource);

    public bool IsNumber
    {
      get { return (bool)GetValue(IsNumberProperty); }
      set { SetValue(IsNumberProperty, value); }
    }
    #endregion

    #region IsRangeProperty

    public static readonly BindableProperty IsRangeProperty =
  BindableProperty.Create(nameof(IsRange), typeof(bool),
    typeof(EntryValidatorBehavior), false, BindingMode.OneWayToSource);

    public bool IsRange
    {
      get { return (bool)GetValue(IsRangeProperty); }
      set { SetValue(IsRangeProperty, value); }
    }
    #endregion

    #region RangeMinProperty

    public static readonly BindableProperty RangeMinProperty =
  BindableProperty.Create(nameof(RangeMin), typeof(Int32),
    typeof(EntryValidatorBehavior), 0, BindingMode.OneWayToSource);

    public Int32 RangeMin
    {
      get { return (Int32)GetValue(RangeMinProperty); }
      set { SetValue(RangeMinProperty, value); }
    }
    #endregion

    #region RangeMaxProperty

    public static readonly BindableProperty RangeMaxProperty =
  BindableProperty.Create(nameof(RangeMax), typeof(Int32),
    typeof(EntryValidatorBehavior), 0, BindingMode.OneWayToSource);

    public Int32 RangeMax
    {
      get { return (Int32)GetValue(RangeMaxProperty); }
      set { SetValue(RangeMaxProperty, value); }
    }
    #endregion

    #region RegExProperty
    public static readonly BindableProperty RegExProperty =
      BindableProperty.Create(nameof(RegEx), typeof(string),
        typeof(EntryValidatorBehavior), "", BindingMode.TwoWay);

    public string RegEx
    {
      get { return (string)GetValue(RegExProperty); }
      set { SetValue(RegExProperty, value); }
    }
    #endregion

    #region BackgroundColorProperty
    public static readonly BindableProperty BackgroundColorProperty =
      BindableProperty.Create(nameof(BackgroundColor), typeof(Color),
        typeof(EntryValidatorBehavior), Color.Red, BindingMode.TwoWay);

    public Color BackgroundColor
    {
      get { return (Color)GetValue(BackgroundColorProperty); }
      set { SetValue(BackgroundColorProperty, value); }
    }
    #endregion

    #region TextColorProperty
    public static readonly BindableProperty TextColorProperty =
      BindableProperty.Create(nameof(TextColor), typeof(Color),
        typeof(EntryValidatorBehavior), Color.White, BindingMode.TwoWay);

    public Color TextColor
    {
      get { return (Color)GetValue(TextColorProperty); }
      set { SetValue(TextColorProperty, value); }
    }
    #endregion

    #region ChangeBackgroundColorProperty
    public static readonly BindableProperty ChangeBackgroundColorProperty =
  BindableProperty.Create(nameof(ChangeBackgroundColor), typeof(bool),
    typeof(EntryValidatorBehavior), true, BindingMode.OneWayToSource);

    public bool ChangeBackgroundColor
    {
      get { return (bool)GetValue(ChangeBackgroundColorProperty); }
      set { SetValue(ChangeBackgroundColorProperty, value); }
    }
    #endregion

    #region ChangeTextColorProperty
    public static readonly BindableProperty ChangeTextColorProperty =
  BindableProperty.Create(nameof(ChangeTextColor), typeof(bool),
    typeof(EntryValidatorBehavior), true, BindingMode.OneWayToSource);

    public bool ChangeTextColor
    {
      get { return (bool)GetValue(ChangeTextColorProperty); }
      set { SetValue(ChangeTextColorProperty, value); }
    }
    #endregion

    #region Overrides
    protected override void OnAttachedTo(Entry bindable)
    {
      bindable.TextChanged += HandleTextChanged;
      base.OnAttachedTo(bindable);
    }

    protected override void OnDetachingFrom(Entry bindable)
    {
      bindable.TextChanged -= HandleTextChanged;
      base.OnDetachingFrom(bindable);
    }
    #endregion

    private bool IsNumeric(string value)
    {
      return double.TryParse(value, out double test);
    }

    private void HandleTextChanged(object sender, TextChangedEventArgs e)
    {
      IsValid = Validate(e.NewTextValue);
      ChangeEntryColor();
    }
    public bool Validate()
    {
      IsValid = Validate(this.AssociatedObject.Text);
      ChangeEntryColor();
      return IsValid;

    }

    private bool Validate(string value)
    {
      if (IsRange)
        IsNumber = true;

      IsRequired = IsRequired || (Length > 0 || IsEmail || IsNumber || IsRange || RegEx != "");


      if (IsRequired & string.IsNullOrWhiteSpace(value))
        return false;

      if (Length > 0 & (value.Length < Length))
        return false;

      if (IsEmail & !Regex.IsMatch(value, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250))
        )
        return false;


      if (RegEx != "" & !Regex.IsMatch(value, RegEx, RegexOptions.None, TimeSpan.FromMilliseconds(250))
        )
        return false;


      if (IsNumber & !IsNumeric(value))
        return false;

      if (IsRange)
      {
        double v = 0;
        if (IsNumeric(value))
        {
          v = Convert.ToDouble(value);
        }
        if (!((v >= RangeMin) && (v <= RangeMax)))
          return false;
      }


      return true;
    }

    private void ChangeEntryColor()
    {
      Entry entry = this.AssociatedObject;
      if (!IsValid)
      {
        if (ChangeBackgroundColor) entry.BackgroundColor = BackgroundColor;
        if (ChangeTextColor) entry.TextColor = TextColor;
      }
      else
      {
        if (ChangeBackgroundColor) entry.BackgroundColor = Color.Default;
        if (ChangeTextColor) entry.TextColor = Color.Default;
      }
    }
  }
}
