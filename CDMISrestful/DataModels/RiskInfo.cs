using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDMISrestful.DataModels
{
    public class RiskInfo
    {
    }
    public class M1Risk
    {       
        public double Hyper { get; set; }
        public double Harvard { get; set; }
        public double Framingham { get; set; }
        public double StrokeRisk { get; set; }
        public double HeartFailureRisk { get; set; }
        public int SBP { get; set; }
        public int DBP { get; set; }

    }
    public class RiskResult
    {
        public string UserId { get; set; }
        public int SortNo { get; set; }
        public string AssessmentType { get; set; }
        public string AssessmentName { get; set; }
        public DateTime AssessmentTime { get; set; }
        public string Result { get; set; }
        public string revUserId { get; set; }
        public string TerminalName { get; set; }
        public string TerminalIP { get; set; }
        public int DeviceType { get; set; }
    }
    public class Parameters
    {
        public string Indicators { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public string Unit { get; set; }
        public string revUserId { get; set; }
        public string TerminalName { get; set; }
        public string TerminalIP { get; set; }
        public int DeviceType { get; set; }
    }
    public class PsTreatmentIndicators
    {
        public int SortNo { get; set; }
        public string AssessmentType { get; set; }
        public string AssessmentName { get; set; }
        public string AssessmentTime { get; set; }
        public string Result { get; set; }
        public string DocName { get; set; }
       
    }

    //public class ValueTime
    //{
    //    public string Value { get; set; }


    //    public string Time { get; set; }
    //}
    
    //public class M1RiskInput
    //{
    //    public int Age { get; set; }

        
    //    public int Gender { get; set; }
    //    public ValueTime Height { get; set; }
    // //   public string HeightTime { get; set; }
    //    public ValueTime Weight { get; set; }
    // //   public string WeightTime { get; set; }
    //    public ValueTime AbdominalGirth { get; set; }
    //  //  public string AbdominalGirthTime { get; set; }
    //    public double BMI { get; set; }
    //    public ValueTime Heartrate { get; set; }
    // //   public string HeartrateTime { get; set; }

    //    public ValueTime Parent { get; set; }

    //  //  public string ParentTime { get; set; }
    //    public ValueTime Smoke { get; set; }
    //  //  public string SmokeTime { get; set; }
    //    public ValueTime Stroke { get; set; }
    // //   public string StrokeTime { get; set; }
    //    public ValueTime Lvh { get; set; }
    // //   public string LvhTime { get; set; }
    //    public ValueTime Diabetes { get; set; }
    // //   public string DiabetesTime { get; set; }
    //    public ValueTime Treat { get; set; }
    //   // public string TreatTime { get; set; }
    //    public ValueTime Heartattack { get; set; }
    //   // public string HeartattackTime { get; set; }
    //    public ValueTime Af { get; set; }
    //   // public string AfTime { get; set; }
    //    public ValueTime Chd { get; set; }
    //   // public string ChdTime { get; set; }
    //    public ValueTime Valve { get; set; }
    //   // public string ValveTime { get; set; }
    //    public ValueTime Tcho { get; set; }
    //   // public string TchoTime { get; set; }
    //    public ValueTime Creatinine { get; set; }
    //   // public string CreatinineTime { get; set; }
    //    public ValueTime Hdlc { get; set; }
    //    //public string HdlcTime { get; set; }
    //    public ValueTime SBP { get; set; }
    //   // public string SBPTime { get; set; }
    //    public ValueTime DBP { get; set; }
    //    //public string DBPTime { get; set; }
    //}

    public class M3RiskInput
    {
        public int Age { get; set; }
        public int Gender { get; set; }
        public int Height { get; set; }
        public string HeightTime { get; set; }
        public int Weight { get; set; }
        public string WeightTime { get; set; }
        public double BMI { get; set; }
        public int Smoke { get; set; }
        public string SmokeTime { get; set; }
        public int Diabetes { get; set; }
        public string DiabetesTime { get; set; }
        public double Creatinine { get; set; }
        public string CreatinineTime { get; set; }
        public int SBP { get; set; }
        public string SBPTime { get; set; }
        public double EF { get; set; }
        public string EFTime { get; set; }
        public int NYHA { get; set; }
        public string NYHATime { get; set; }
        public int Lung { get; set; }
        public string LungTime { get; set; }
        public double HF18 { get; set; }
        public string HF18Time { get; set; }

        public int Beta { get; set; }
        public string BetaTime { get; set; }
        public int AA { get; set; }
        public string AATime { get; set; }
    }

    public class M3Risk
    {
        public double One { get; set; }
        public double Three { get; set; }

    }

    public class M1RiskInput
    {

        public int Age { get; set; }
        public int Gender { get; set; }
        public int Height { get; set; }
        public string HeightTime { get; set; }
        public int Weight { get; set; }
        public string WeightTime { get; set; }
        public int AbdominalGirth { get; set; }
        public string AbdominalGirthTime { get; set; }
        public double BMI { get; set; }
        public int Heartrate { get; set; }
        public string HeartrateTime { get; set; }
        public int Parent { get; set; }
        public string ParentTime { get; set; }
        public int Smoke { get; set; }
        public string SmokeTime { get; set; }
        public int Stroke { get; set; }
        public string StrokeTime { get; set; }
        public int Lvh { get; set; }
        public string LvhTime { get; set; }
        public int Diabetes { get; set; }
        public string DiabetesTime { get; set; }
        public int Treat { get; set; }
        public string TreatTime { get; set; }
        public int Heartattack { get; set; }
        public string HeartattackTime { get; set; }
        public int Af { get; set; }
        public string AfTime { get; set; }
        public int Chd { get; set; }
        public string ChdTime { get; set; }
        public int Valve { get; set; }
        public string ValveTime { get; set; }
        public double Tcho { get; set; }
        public string TchoTime { get; set; }
        public double Creatinine { get; set; }
        public string CreatinineTime { get; set; }
        public double Hdlc { get; set; }
        public string HdlcTime { get; set; }
        public int SBP { get; set; }
        public string SBPTime { get; set; }
        public int DBP { get; set; }
        public string DBPTime { get; set; }
    }

}