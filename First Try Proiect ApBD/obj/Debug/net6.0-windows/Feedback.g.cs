﻿#pragma checksum "..\..\..\Feedback.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "306BD78A65ADF47387E5D85D873B582D33F36B99"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using First_Try_Proiect_ApBD;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace First_Try_Proiect_ApBD {
    
    
    /// <summary>
    /// Feedback
    /// </summary>
    public partial class Feedback : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 17 "..\..\..\Feedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Canvas StarCanvas;
        
        #line default
        #line hidden
        
        
        #line 18 "..\..\..\Feedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Polygon star1;
        
        #line default
        #line hidden
        
        
        #line 19 "..\..\..\Feedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Polygon star2;
        
        #line default
        #line hidden
        
        
        #line 20 "..\..\..\Feedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Polygon star3;
        
        #line default
        #line hidden
        
        
        #line 21 "..\..\..\Feedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Polygon star4;
        
        #line default
        #line hidden
        
        
        #line 22 "..\..\..\Feedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Polygon star5;
        
        #line default
        #line hidden
        
        
        #line 24 "..\..\..\Feedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_SEND;
        
        #line default
        #line hidden
        
        
        #line 25 "..\..\..\Feedback.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button Btn_DONT;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.12.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/First Try Proiect ApBD;component/feedback.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\Feedback.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.12.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.StarCanvas = ((System.Windows.Controls.Canvas)(target));
            return;
            case 2:
            this.star1 = ((System.Windows.Shapes.Polygon)(target));
            
            #line 18 "..\..\..\Feedback.xaml"
            this.star1.MouseEnter += new System.Windows.Input.MouseEventHandler(this.Star_MouseEnter);
            
            #line default
            #line hidden
            
            #line 18 "..\..\..\Feedback.xaml"
            this.star1.MouseLeave += new System.Windows.Input.MouseEventHandler(this.Star_MouseLeave);
            
            #line default
            #line hidden
            
            #line 18 "..\..\..\Feedback.xaml"
            this.star1.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Star_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 3:
            this.star2 = ((System.Windows.Shapes.Polygon)(target));
            
            #line 19 "..\..\..\Feedback.xaml"
            this.star2.MouseEnter += new System.Windows.Input.MouseEventHandler(this.Star_MouseEnter);
            
            #line default
            #line hidden
            
            #line 19 "..\..\..\Feedback.xaml"
            this.star2.MouseLeave += new System.Windows.Input.MouseEventHandler(this.Star_MouseLeave);
            
            #line default
            #line hidden
            
            #line 19 "..\..\..\Feedback.xaml"
            this.star2.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Star_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 4:
            this.star3 = ((System.Windows.Shapes.Polygon)(target));
            
            #line 20 "..\..\..\Feedback.xaml"
            this.star3.MouseEnter += new System.Windows.Input.MouseEventHandler(this.Star_MouseEnter);
            
            #line default
            #line hidden
            
            #line 20 "..\..\..\Feedback.xaml"
            this.star3.MouseLeave += new System.Windows.Input.MouseEventHandler(this.Star_MouseLeave);
            
            #line default
            #line hidden
            
            #line 20 "..\..\..\Feedback.xaml"
            this.star3.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Star_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 5:
            this.star4 = ((System.Windows.Shapes.Polygon)(target));
            
            #line 21 "..\..\..\Feedback.xaml"
            this.star4.MouseEnter += new System.Windows.Input.MouseEventHandler(this.Star_MouseEnter);
            
            #line default
            #line hidden
            
            #line 21 "..\..\..\Feedback.xaml"
            this.star4.MouseLeave += new System.Windows.Input.MouseEventHandler(this.Star_MouseLeave);
            
            #line default
            #line hidden
            
            #line 21 "..\..\..\Feedback.xaml"
            this.star4.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Star_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 6:
            this.star5 = ((System.Windows.Shapes.Polygon)(target));
            
            #line 22 "..\..\..\Feedback.xaml"
            this.star5.MouseEnter += new System.Windows.Input.MouseEventHandler(this.Star_MouseEnter);
            
            #line default
            #line hidden
            
            #line 22 "..\..\..\Feedback.xaml"
            this.star5.MouseLeave += new System.Windows.Input.MouseEventHandler(this.Star_MouseLeave);
            
            #line default
            #line hidden
            
            #line 22 "..\..\..\Feedback.xaml"
            this.star5.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Star_MouseLeftButtonDown);
            
            #line default
            #line hidden
            return;
            case 7:
            this.Btn_SEND = ((System.Windows.Controls.Button)(target));
            
            #line 24 "..\..\..\Feedback.xaml"
            this.Btn_SEND.Click += new System.Windows.RoutedEventHandler(this.SendButton_Click);
            
            #line default
            #line hidden
            return;
            case 8:
            this.Btn_DONT = ((System.Windows.Controls.Button)(target));
            
            #line 25 "..\..\..\Feedback.xaml"
            this.Btn_DONT.Click += new System.Windows.RoutedEventHandler(this.DontSendButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

