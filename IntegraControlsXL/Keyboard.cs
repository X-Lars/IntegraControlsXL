using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IntegraControlsXL
{
    public enum KeyboardButtonTypes
    {
        Whole,
        Half
    }

    public class KeyboardButton : Button
    {
        public int NoteNumber { get; set; }

        static KeyboardButton() { }

        public KeyboardButton(KeyboardButtonTypes type = KeyboardButtonTypes.Whole)
        {
            BorderThickness = new Thickness(0.5);

            if (type == KeyboardButtonTypes.Whole)
            {
                Background = Brushes.White;
                Width = 20;
                Height = 80;
            }
            else
            {
                Background = Brushes.Black;
                Width = 15;
                Height = 50;
            }
        }
    }

    public class KeyboardControlEventArgs : RoutedEventArgs
    {
        private int _NoteNumber;
        private int _Velocity;

        public KeyboardControlEventArgs(RoutedEvent routedEvent, int noteNumber, int velocity) : base(routedEvent)
        {
            _NoteNumber = noteNumber;
            _Velocity = velocity;
        }

        public int NoteNumber { get { return _NoteNumber; } }
        public int Velocity { get { return _Velocity; } }
    }

    public class Keyboard : Control
    {
        private Collection<KeyboardButton> _Keys = new Collection<KeyboardButton>();

        private Dictionary<int, string> _KeyNames = new Dictionary<int, string>();
        private Canvas _Canvas;

        public static readonly DependencyProperty OctavesProperty = DependencyProperty.Register("Octaves", typeof(int), typeof(Keyboard), new PropertyMetadata(4));
        public static readonly DependencyProperty OctaveOffsetProperty = DependencyProperty.Register("OctaveOffset", typeof(int), typeof(Keyboard), new PropertyMetadata(2));
        public static readonly DependencyProperty VelocityProperty = DependencyProperty.Register("Velocity", typeof(int), typeof(Keyboard), new PropertyMetadata(100));

        public static readonly RoutedEvent NoteOnEvent = EventManager.RegisterRoutedEvent("NoteOn", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Keyboard));
        public static readonly RoutedEvent NoteOffEvent = EventManager.RegisterRoutedEvent("NoteOff", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(Keyboard));

        // Provides access to the event from XAML
        public event RoutedEventHandler NoteOn
        {
            add { AddHandler(NoteOnEvent, value); }
            remove { RemoveHandler(NoteOnEvent, value); }
        }

        public event RoutedEventHandler NoteOff
        {
            add { AddHandler(NoteOffEvent, value); }
            remove { RemoveHandler(NoteOffEvent, value); }
        }

        public int Octaves
        {
            get { return (int)GetValue(OctavesProperty); }
            set
            {
                value = Math.Min(value, 11);
                value = Math.Max(value, 1);

                SetValue(OctavesProperty, value);
            }
        }

        public int OctaveOffset
        {
            get
            {
                return (int)GetValue(OctaveOffsetProperty);
            }
            set
            {

                value = Math.Min(value, 9);
                value = Math.Max(value, -1);

                SetValue(OctaveOffsetProperty, value);
            }
        }

        public int Velocity
        {
            get { return (int)GetValue(VelocityProperty); }
            set { SetValue(VelocityProperty, value); }
        }

        static Keyboard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(Keyboard), new FrameworkPropertyMetadata(typeof(Keyboard)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _Canvas = GetTemplateChild("PART_Content") as Canvas;

            InitializeKeyNames();
            InitializeKeyboard();
        }

        private void InitializeKeyNames()
        {
            _KeyNames.Add(0, "C");
            _KeyNames.Add(1, "C#");
            _KeyNames.Add(2, "D");
            _KeyNames.Add(3, "Eb");
            _KeyNames.Add(4, "E");
            _KeyNames.Add(5, "F");
            _KeyNames.Add(6, "F#");
            _KeyNames.Add(7, "G");
            _KeyNames.Add(8, "G#");
            _KeyNames.Add(9, "A");
            _KeyNames.Add(10, "Bb");
            _KeyNames.Add(11, "B");
        }

        private void InitializeKeyboard()
        {
            double offsetX;
            bool isMaxTone = false;

            KeyboardButton keyboardReference = new KeyboardButton();
            _Canvas.Width = 0;

            for (int octave = 0; octave < Octaves; octave++)
            {
                // Initialize the offset based on the current octave
                offsetX = octave * 7 * keyboardReference.Width;

                // Whole tone keys
                for (int tone = 0; tone < 12; tone++)
                {
                    // Break if the max tone is reached
                    if ((octave * 12) + ((OctaveOffset + 1) * 12) + tone > 127)
                    {
                        isMaxTone = true;
                        break;
                    }

                    // Add the whole tone keys to the keyboard
                    switch (tone)
                    {
                        case 0:
                        case 2:
                        case 4:
                        case 5:
                        case 7:
                        case 9:
                        case 11:

                            KeyboardButton button = new KeyboardButton();

                            button.NoteNumber = (octave * 12) + ((OctaveOffset + 1) * 12) + tone;
                            button.ToolTip = $"{_KeyNames[button.NoteNumber % 12]} {octave + OctaveOffset}";

                            // Use preview, normal mouse down is captured by the canvas, not the button
                            button.PreviewMouseDown += KeyboardButtonMouseDown;
                            button.PreviewMouseUp += KeyboardButtonMouseUp;
                            Canvas.SetLeft(button, offsetX);
                            Canvas.SetTop(button, 0);

                            _Canvas.Children.Add(button);

                            offsetX += keyboardReference.Width;
                            _Canvas.Width += keyboardReference.Width;
                            break;
                    }

                }

                // Initialize the offset based on the current octave
                offsetX = (octave * 7 * keyboardReference.Width) + (keyboardReference.Width / 2);

                // Semi tone keys
                for (int tone = 0; tone < 12; tone++)
                {
                    // Break if the max tone is reached
                    if ((octave * 12) + ((OctaveOffset + 1) * 12) + tone > 127)
                    {
                        break;
                    }

                    // Add the semi tone keys to the keyboard
                    switch (tone)
                    {
                        case 1:
                        case 3:
                        case 6:
                        case 8:
                        case 10:

                            KeyboardButton button = new KeyboardButton(KeyboardButtonTypes.Half);

                            button.NoteNumber = (octave * 12) + ((OctaveOffset + 1) * 12) + tone;
                            button.ToolTip = $"{_KeyNames[button.NoteNumber % 12]} {octave + OctaveOffset}";

                            // Use preview, normal mouse down is captured by the canvas, not the button
                            button.PreviewMouseDown += KeyboardButtonMouseDown;
                            button.PreviewMouseUp += KeyboardButtonMouseUp;

                            Canvas.SetLeft(button, offsetX);
                            Canvas.SetTop(button, 0);

                            _Canvas.Children.Add(button);

                            if (tone == 3)
                            {
                                offsetX += keyboardReference.Width * 2;
                            }
                            else
                            {
                                offsetX += keyboardReference.Width;
                            }

                            break;
                    }

                }

                // Last C tone
                if (octave == Octaves - 1)
                {
                    if (!isMaxTone)
                    {
                        offsetX = (octave + 1) * 7 * keyboardReference.Width;

                        KeyboardButton button = new KeyboardButton();

                        button.NoteNumber = ((octave + 1) * 12) + ((OctaveOffset + 1) * 12);
                        button.ToolTip = $"{_KeyNames[button.NoteNumber % 12]} {octave + OctaveOffset}";

                        // Use preview, normal mouse down is captured by the canvas, not the button
                        button.PreviewMouseDown += KeyboardButtonMouseDown;
                        button.PreviewMouseUp += KeyboardButtonMouseUp;

                        Canvas.SetLeft(button, offsetX);
                        Canvas.SetTop(button, 0);

                        _Canvas.Children.Add(button);
                        _Canvas.Width += keyboardReference.Width;
                    }
                }
            }

            _Canvas.Height = keyboardReference.Height;

            InvalidateVisual();
        }

        private void KeyboardButtonMouseUp(object sender, MouseButtonEventArgs e)
        {
            KeyboardButton button = sender as KeyboardButton;
            RaiseEvent(new KeyboardControlEventArgs(Keyboard.NoteOffEvent, button.NoteNumber, Velocity));
        }

        private void KeyboardButtonMouseDown(object sender, MouseButtonEventArgs e)
        {
            KeyboardButton button = sender as KeyboardButton;
            RaiseEvent(new KeyboardControlEventArgs(Keyboard.NoteOnEvent, button.NoteNumber, Velocity));
        }
    }
}
