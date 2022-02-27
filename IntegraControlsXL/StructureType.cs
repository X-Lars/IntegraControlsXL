using System;
using System.Collections.Generic;
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
using IntegraXL.Core;

namespace IntegraControlsXL
{
   
    public enum StructurePartials
    {
        Partial12,
        Partial34
    }
    public class StructureType : Control
    {
        private Border _WG1_B0;
        private Border _WG1_B1;
        private Border _WG1_B2;
        private Border _WG1_B3;
        private Border _WG1_B4;
        private Border _WG1_B5;

        private Border _C_B1;
        private Border _C_B2;

        private Border _WG2_B0;
        private Border _WG2_B1;
        private Border _WG2_B2;
        private Border _WG2_B3;
        private Border _WG2_B4;
        private Border _WG2_B5;

        private TextBlock _WG1_T1;
        private TextBlock _WG1_T2;
        private TextBlock _WG1_T3;
        private TextBlock _WG1_T4;
        private TextBlock _WG1_T5;

        private TextBlock _C_T1;
        private TextBlock _C_T2;

        private TextBlock _WG2_T1;
        private TextBlock _WG2_T2;
        private TextBlock _WG2_T3;
        private TextBlock _WG2_T4;
        private TextBlock _WG2_T5;

        private TextBlock _WG1_Text;
        private TextBlock _WG2_Text;

        static StructureType()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StructureType), new FrameworkPropertyMetadata(typeof(StructureType)));
        }


        public StructureType()
        {
            Loaded += (s, e) => Update();
        }


        public IntegraStructureType Type
        {
            get { return (IntegraStructureType)GetValue(TypeProperty); }
            set { SetValue(TypeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Type.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TypeProperty = DependencyProperty.Register(nameof(Type), typeof(IntegraStructureType), typeof(StructureType), new PropertyMetadata(IntegraStructureType.Type1, TypeChanged));

        private static void TypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((StructureType)d).Update();

        }



        public StructurePartials Partials
        {
            get { return (StructurePartials)GetValue(PartialsProperty); }
            set { SetValue(PartialsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Partials.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PartialsProperty = DependencyProperty.Register(nameof(Partials), typeof(StructurePartials), typeof(StructureType), new PropertyMetadata(StructurePartials.Partial12));



        private void Update()
        {
            if (!IsLoaded)
                return;

            P10O = _WG1_B0.TransformToAncestor(this).Transform(new Point(_WG1_B0.ActualWidth, _WG1_B0.ActualHeight / 2));
            P11I = _WG1_B1.TransformToAncestor(this).Transform(new Point(0, _WG1_B1.ActualHeight / 2));
            P11O = _WG1_B1.TransformToAncestor(this).Transform(new Point(_WG1_B1.ActualWidth, _WG1_B1.ActualHeight / 2));
            P12I = _WG1_B2.TransformToAncestor(this).Transform(new Point(0, _WG1_B1.ActualHeight / 2));
            P12O = _WG1_B2.TransformToAncestor(this).Transform(new Point(_WG1_B1.ActualWidth, _WG1_B2.ActualHeight / 2));
            P13I = _WG1_B3.TransformToAncestor(this).Transform(new Point(0, _WG1_B3.ActualHeight / 2));
            P13O = _WG1_B3.TransformToAncestor(this).Transform(new Point(_WG1_B3.ActualWidth, _WG1_B3.ActualHeight / 2));
            P15I = _WG1_B5.TransformToAncestor(this).Transform(new Point(0, _WG1_B5.ActualHeight / 2));

            PC1I = _C_B1.TransformToAncestor(this).Transform(new Point(0, _C_B1.ActualHeight / 2));
            PC1O = _C_B1.TransformToAncestor(this).Transform(new Point(_C_B1.ActualWidth, _C_B1.ActualHeight / 2));
            PC2I = _C_B2.TransformToAncestor(this).Transform(new Point(0, _C_B2.ActualHeight / 2));
            PC2O = _C_B2.TransformToAncestor(this).Transform(new Point(_C_B2.ActualWidth, _C_B2.ActualHeight / 2));

            P20O = _WG2_B0.TransformToAncestor(this).Transform(new Point(_WG2_B0.ActualWidth, _WG2_B0.ActualHeight / 2));
            P21I = _WG2_B1.TransformToAncestor(this).Transform(new Point(0, _WG2_B1.ActualHeight / 2));
            P21O = _WG2_B1.TransformToAncestor(this).Transform(new Point(_WG2_B1.ActualWidth, _WG2_B1.ActualHeight / 2));
            P22I = _WG2_B2.TransformToAncestor(this).Transform(new Point(0, _WG2_B2.ActualHeight / 2));
            P22O = _WG2_B2.TransformToAncestor(this).Transform(new Point(_WG2_B2.ActualWidth, _WG2_B2.ActualHeight / 2));
            P23I = _WG2_B3.TransformToAncestor(this).Transform(new Point(0, _WG2_B3.ActualHeight / 2));
            P23O = _WG2_B3.TransformToAncestor(this).Transform(new Point(_WG2_B3.ActualWidth, _WG2_B3.ActualHeight / 2));
            P24I = _WG2_B4.TransformToAncestor(this).Transform(new Point(0, _WG2_B4.ActualHeight / 2));
            P24O = _WG2_B4.TransformToAncestor(this).Transform(new Point(_WG2_B4.ActualWidth, _WG2_B4.ActualHeight / 2));
            P25I = _WG2_B5.TransformToAncestor(this).Transform(new Point(0, _WG2_B5.ActualHeight / 2));

            switch (Type)
            {
                case IntegraStructureType.Type1:

                    _WG1_B3.Visibility = _WG1_B5.Visibility = Visibility.Visible;
                    _WG2_B3.Visibility = _WG2_B5.Visibility = Visibility.Visible;
                    _WG1_B1.Visibility = _WG1_B2.Visibility = _WG1_B4.Visibility = Visibility.Hidden;
                    _WG2_B1.Visibility = _WG2_B2.Visibility = _WG2_B4.Visibility = Visibility.Hidden;

                    _C_B1.Visibility = _C_B2.Visibility = Visibility.Hidden;

                    _WG1_T3.Text = _WG2_T3.Text = "TVF";
                    _WG1_T5.Text = _WG2_T5.Text = "TVA";

                    break;

                case IntegraStructureType.Type2:

                    _WG1_B1.Visibility = _WG1_B3.Visibility = Visibility.Visible;
                    _WG2_B4.Visibility = _WG2_B5.Visibility = Visibility.Visible;
                    _WG1_B2.Visibility = _WG1_B4.Visibility = _WG1_B5.Visibility = Visibility.Hidden;
                    _WG2_B1.Visibility = _WG2_B2.Visibility = _WG2_B3.Visibility = Visibility.Hidden;

                    _C_B1.Visibility = _C_B2.Visibility = Visibility.Hidden;

                    _WG1_T3.Text = "TVF";

                    break;
                case IntegraStructureType.Type3:

                    _WG1_B1.Visibility = _WG1_B2.Visibility = Visibility.Visible;
                    _WG2_B4.Visibility = _WG2_B5.Visibility = Visibility.Visible;
                    _WG1_B3.Visibility = _WG1_B4.Visibility = _WG1_B5.Visibility = Visibility.Hidden;
                    _WG2_B1.Visibility = _WG2_B2.Visibility = _WG2_B3.Visibility = Visibility.Hidden;

                    _C_B2.Visibility = Visibility.Visible;
                    _C_B1.Visibility =  Visibility.Hidden;

                    _WG1_T2.Text = "TVF";
                    _C_T2.Text = "B";

                    break;

                case IntegraStructureType.Type4:

                    _WG1_B1.Visibility = _WG1_B3.Visibility = Visibility.Visible;
                    _WG2_B4.Visibility = _WG2_B5.Visibility = Visibility.Visible;

                    _WG1_B2.Visibility = _WG1_B4.Visibility = _WG1_B5.Visibility = Visibility.Hidden;
                    _WG2_B1.Visibility = _WG2_B2.Visibility = _WG2_B3.Visibility = Visibility.Hidden;

                    _C_B1.Visibility = Visibility.Visible;
                    _C_B2.Visibility = Visibility.Hidden;

                    _WG1_T3.Text = "TVF";
                    _C_T1.Text = "B";

                    break;

                case IntegraStructureType.Type5:
                case IntegraStructureType.Type6:

                    _WG1_B1.Visibility = _WG1_B3.Visibility = Visibility.Visible;
                    _WG2_B4.Visibility = _WG2_B5.Visibility = Visibility.Visible;

                    _WG1_B2.Visibility = _WG1_B4.Visibility = _WG1_B5.Visibility = Visibility.Hidden;
                    _WG2_B1.Visibility = _WG2_B2.Visibility = _WG2_B3.Visibility = Visibility.Hidden;

                    _C_B1.Visibility = Visibility.Visible;
                    _C_B2.Visibility = Visibility.Hidden;

                    _WG1_T3.Text = "TVF";
                    _C_T1.Text = "R";

                    break;

                case IntegraStructureType.Type7:
                case IntegraStructureType.Type8:

                    _WG1_B1.Visibility = _WG1_B2.Visibility = Visibility.Visible;
                    _WG2_B4.Visibility = _WG2_B5.Visibility = Visibility.Visible;

                    _WG1_B3.Visibility = _WG1_B4.Visibility = _WG1_B5.Visibility = Visibility.Hidden;
                    _WG2_B1.Visibility = _WG2_B2.Visibility = _WG2_B3.Visibility = Visibility.Hidden;

                    _C_B2.Visibility = Visibility.Visible;
                    _C_B1.Visibility = Visibility.Hidden;

                    _WG1_T1.Text = "TVF";
                    _C_T2.Text = "R";

                    break;

                case IntegraStructureType.Type9:
                case IntegraStructureType.Type10:

                    _WG1_B1.Visibility = _WG1_B2.Visibility = Visibility.Visible;
                    _WG2_B1.Visibility = _WG2_B5.Visibility = Visibility.Visible;

                    _WG1_B3.Visibility = _WG1_B4.Visibility = _WG1_B5.Visibility = Visibility.Hidden;
                    _WG2_B2.Visibility = _WG2_B3.Visibility = _WG2_B4.Visibility = Visibility.Hidden;

                    _C_B2.Visibility = Visibility.Visible;
                    _C_B1.Visibility = Visibility.Hidden;

                    _WG1_T1.Text = "TVF";
                    _C_T2.Text = "R";

                    break;
            }

            InvalidateVisual();
        }

        private Point P10O, P11I, P11O, P12I, P12O, P13I, P13O, P15I;
        private Point P20O, P21I, P21O, P22I, P22O, P23I, P23O, P24I, P24O, P25I;
        private Point PC1I, PC1O, PC2I, PC2O;

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            Update();
        }

        protected override void OnRender(DrawingContext dc)
        {
            base.OnRender(dc);

            Pen pen = Styles.GraphSelectedPen;

            switch (Type)
            {
                case IntegraStructureType.Type1:

                    dc.DrawLine(pen, P10O, P13I);
                    dc.DrawLine(pen, P13O, P15I);
                    dc.DrawLine(pen, P20O, P23I);
                    dc.DrawLine(pen, P23O, P25I);

                    break;

                case IntegraStructureType.Type2:

                    dc.DrawLine(pen, P10O, P11I);
                    dc.DrawLine(pen, P11O, P13I);
                    dc.DrawLine(pen, P13O, P24I);
                    dc.DrawLine(pen, P24O, P25I);

                    dc.DrawLine(pen, P20O, P21O);
                    dc.DrawLine(pen, P21O, P13I);

                    break;

                case IntegraStructureType.Type3:

                    dc.DrawLine(pen, P10O, P11I);
                    dc.DrawLine(pen, P11O, P12I);
                    dc.DrawLine(pen, P12O, PC2I);
                    dc.DrawLine(pen, PC2O, new Point(PC2O.X + (P24I.X - PC2O.X) / 2, PC2O.Y));
                    dc.DrawLine(pen, new Point(PC2O.X + (P24I.X - PC2O.X) / 2, PC2O.Y), P24I);
                    dc.DrawLine(pen, P24O, P25I);

                    dc.DrawLine(pen, P20O, P22O);
                    dc.DrawLine(pen, P22O, PC2I);

                    break;

                case IntegraStructureType.Type4:
                case IntegraStructureType.Type5:

                    dc.DrawLine(pen, P10O, P11I);
                    dc.DrawLine(pen, P11O, PC1I);
                    dc.DrawLine(pen, PC1O, new Point(PC1O.X + (P13I.X - PC1O.X) / 2, PC1O.Y));
                    dc.DrawLine(pen, new Point(PC1O.X + (P13I.X - PC1O.X) / 2, PC1O.Y), P13I);
                    dc.DrawLine(pen, P13O, P24I);
                    dc.DrawLine(pen, P24O, P25I);

                    dc.DrawLine(pen, P20O, P21O);
                    dc.DrawLine(pen, P21O, PC1I);

                    break;

                case IntegraStructureType.Type6:

                    dc.DrawLine(pen, P10O, P11I);
                    dc.DrawLine(pen, P11O, PC1I);
                    dc.DrawLine(pen, PC1O, new Point(PC1O.X + (P13I.X - PC1O.X) / 2, PC1O.Y));
                    dc.DrawLine(pen, new Point(PC1O.X + (P13I.X - PC1O.X) / 2, PC1O.Y), P13I);
                    dc.DrawLine(pen, P13O, P24I);
                    dc.DrawLine(pen, P24O, P25I);

                    dc.DrawLine(pen, P20O, P21O);
                    dc.DrawLine(pen, P21O, PC1I);
                    dc.DrawLine(pen, P21O, new Point(PC1O.X + (P13I.X - PC1O.X) / 2, P21O.Y));
                    dc.DrawLine(pen, new Point(PC1O.X + (P13I.X - PC1O.X) / 2, P21O.Y), new Point(PC1O.X + (P13I.X - PC1O.X) / 2, PC1O.Y));

                    break;
                case IntegraStructureType.Type7:
                    dc.DrawLine(pen, P10O, P11I);
                    dc.DrawLine(pen, P11O, P12I);
                    dc.DrawLine(pen, P12O, PC2I);

                    dc.DrawLine(pen, P20O, P22O);
                    dc.DrawLine(pen, P22O, PC2I);
                    dc.DrawLine(pen, PC2O, P24I);
                    dc.DrawLine(pen, P24O, P25I);


                    break;
                case IntegraStructureType.Type8:

                    dc.DrawLine(pen, P10O, P11I);
                    dc.DrawLine(pen, P11O, P12I);
                    dc.DrawLine(pen, P12O, PC2I);

                    dc.DrawLine(pen, P20O, P22O);
                    dc.DrawLine(pen, P22O, PC2I);
                    dc.DrawLine(pen, PC2O, P24I);
                    dc.DrawLine(pen, P24O, P25I);

                    dc.DrawLine(pen, P22O, P24I);
                    break;

                case IntegraStructureType.Type9:

                    dc.DrawLine(pen, P10O, P11I);
                    dc.DrawLine(pen, P11O, P12I);
                    dc.DrawLine(pen, P12O, PC2I);

                    dc.DrawLine(pen, P20O, P21I);
                    dc.DrawLine(pen, P21O, P22O);
                    dc.DrawLine(pen, P22O, PC2I);
                    dc.DrawLine(pen, PC2O, new Point(P24O.X, PC2O.Y));
                    dc.DrawLine(pen, new Point(P24O.X, PC2O.Y), P25I);

                    break;

                case IntegraStructureType.Type10:

                    dc.DrawLine(pen, P10O, P11I);
                    dc.DrawLine(pen, P11O, P12I);
                    dc.DrawLine(pen, P12O, PC2I);

                    dc.DrawLine(pen, P20O, P21I);
                    dc.DrawLine(pen, P21O, P22O);
                    dc.DrawLine(pen, P22O, PC2I);
                    dc.DrawLine(pen, PC2O, new Point(P24O.X, PC2O.Y));
                    dc.DrawLine(pen, new Point(P24O.X, PC2O.Y), P25I);
                    dc.DrawLine(pen, P22O, P25I);
                    break;
            }

        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _WG1_Text = GetTemplateChild("PART_WG1_Text") as TextBlock;
            _WG2_Text = GetTemplateChild("PART_WG2_Text") as TextBlock;

            _WG1_B0 = GetTemplateChild("PART_WG1_B0") as Border;
            _WG1_B1 = GetTemplateChild("PART_WG1_B1") as Border;
            _WG1_B2 = GetTemplateChild("PART_WG1_B2") as Border;
            _WG1_B3 = GetTemplateChild("PART_WG1_B3") as Border;
            _WG1_B4 = GetTemplateChild("PART_WG1_B4") as Border;
            _WG1_B5 = GetTemplateChild("PART_WG1_B5") as Border;

            _C_B1 = GetTemplateChild("PART_C_B1") as Border;
            _C_B2 = GetTemplateChild("PART_C_B2") as Border;

            _WG2_B0 = GetTemplateChild("PART_WG2_B0") as Border;
            _WG2_B1 = GetTemplateChild("PART_WG2_B1") as Border;
            _WG2_B2 = GetTemplateChild("PART_WG2_B2") as Border;
            _WG2_B3 = GetTemplateChild("PART_WG2_B3") as Border;
            _WG2_B4 = GetTemplateChild("PART_WG2_B4") as Border;
            _WG2_B5 = GetTemplateChild("PART_WG2_B5") as Border;

            _WG1_T1 = GetTemplateChild("PART_WG1_T1") as TextBlock;
            _WG1_T2 = GetTemplateChild("PART_WG1_T2") as TextBlock;
            _WG1_T3 = GetTemplateChild("PART_WG1_T3") as TextBlock;
            _WG1_T4 = GetTemplateChild("PART_WG1_T4") as TextBlock;
            _WG1_T5 = GetTemplateChild("PART_WG1_T5") as TextBlock;

            _C_T1 = GetTemplateChild("PART_C_T1") as TextBlock;
            _C_T2 = GetTemplateChild("PART_C_T2") as TextBlock;

            _WG2_T1 = GetTemplateChild("PART_WG2_T1") as TextBlock;
            _WG2_T2 = GetTemplateChild("PART_WG2_T2") as TextBlock;
            _WG2_T3 = GetTemplateChild("PART_WG2_T3") as TextBlock;
            _WG2_T4 = GetTemplateChild("PART_WG2_T4") as TextBlock;
            _WG2_T5 = GetTemplateChild("PART_WG2_T5") as TextBlock;

            if(Partials == StructurePartials.Partial12)
            {
                _WG1_Text.Text = "Partial 1";
                _WG2_Text.Text = "Partial 2";
            }
            else
            {
                _WG1_Text.Text = "Partial 3";
                _WG2_Text.Text = "Partial 4";
            }
        }
    }
}
