//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace app_login
{
    using System;
    using System.Collections.Generic;
    
    public partial class Scheduling
    {
        public int ID_Meeting { get; set; }
        public Nullable<int> ID_Prof { get; set; }
        public Nullable<int> ID_Std { get; set; }
        public Nullable<int> ID_Subj { get; set; }
        public Nullable<System.DateTime> Programare { get; set; }
        public string StatusProgramare { get; set; }
        public Nullable<int> ProgresSTD { get; set; }
        public Nullable<int> EVALProf { get; set; }
    
        public virtual Profesor Profesor { get; set; }
        public virtual Student Student { get; set; }
        public virtual Subject Subject { get; set; }
    }
}