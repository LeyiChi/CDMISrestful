using CDMISrestful.CommonLibrary;
using CDMISrestful.DataMethod;
using CDMISrestful.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDMISrestful.Models
{
    public class RiskInfoRepository : IRiskInfoRepository
    {

        /// <summary>
        /// 根据收缩压获取血压等级说明 LY 2015-10-13
        /// </summary>
        /// <param name="SBP"></param>
        /// <returns></returns>
        public string GetDescription(DataConnection pclsCache, int SBP)
        {
            return new PlanInfoMethod().GetDescription(pclsCache, SBP);
        }

        /// <summary>
        /// 插入风险评估结果 LY 2015-10-13
        /// </summary>
        /// <param name="UserId"></param>
        /// <param name="AssessmentType"></param>
        /// <param name="AssessmentName"></param>
        /// <param name="AssessmentTime"></param>
        /// <param name="Result"></param>
        /// <param name="revUserId"></param>
        /// <param name="TerminalName"></param>
        /// <param name="TerminalIP"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        public int SetRiskResult(DataConnection pclsCache, string UserId, string AssessmentType, string AssessmentName, DateTime AssessmentTime, string Result, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            int SortNo = new RiskInfoMethod().GetMaxSortNo(pclsCache, UserId) + 1;    //SortNo自增
            return new RiskInfoMethod().PsTreatmentIndicatorsSetData(pclsCache, UserId, SortNo, AssessmentType, AssessmentName, AssessmentTime, Result, revUserId, TerminalName, TerminalIP, DeviceType);
        }

        public int PsTreatmentIndicatorsSetData(DataConnection pclsCache, string UserId, int SortNo, string AssessmentType, string AssessmentName, DateTime AssessmentTime, string Result, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            //int SortNo = new RiskInfoMethod().GetMaxSortNo(pclsCache, UserId) + 1;    //SortNo自增
            return new RiskInfoMethod().PsTreatmentIndicatorsSetData(pclsCache, UserId, SortNo, AssessmentType, AssessmentName, AssessmentTime, Result, revUserId, TerminalName, TerminalIP, DeviceType);
        }
        public int GetMaxSortNo(DataConnection pclsCache, string UserId)
        {
            //int SortNo = new RiskInfoMethod().GetMaxSortNo(pclsCache, UserId) + 1;    //SortNo自增
            return new RiskInfoMethod().GetMaxSortNo(pclsCache, UserId);
        }
        public int PsParametersSetData(DataConnection pclsCache, string Indicators, string Id, string Name, string Value, string Unit, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            return new RiskInfoMethod().PsParametersSetData(pclsCache, Indicators, Id, Name, Value, Unit, revUserId, TerminalName, TerminalIP, DeviceType);
        }
        /// <summary>
        /// 根据UserId获取最新风险评估结果 LY 2015-10-13
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public string GetRiskResult(DataConnection pclsCache, string UserId, string AssessmentType)
        {
            int SortNo = new RiskInfoMethod().GetMaxSortNo(pclsCache, UserId);
            return new RiskInfoMethod().GetResult(pclsCache, UserId, SortNo, AssessmentType);
        }

       
        public List<PsTreatmentIndicators> GetPsTreatmentIndicators(DataConnection pclsCache, string UserId)
        {
            return new RiskInfoMethod().GetPsTreatmentIndicators(pclsCache, UserId);

        }
        public List<Parameters> GetParameters(DataConnection pclsCache, string Indicators)
        {
            return new RiskInfoMethod().GetParameters(pclsCache, Indicators);

        }

        public M1RiskInput GetM1RiskInput(DataConnection pclsCache, string UserId)
        {
            M1RiskInput Input = new M1RiskInput();
            Input = new UsersMethod().GetM1RiskInput(pclsCache, UserId);
            if(Input != null)
            {
                BasicInfo BaseList = new UsersMethod().GetBasicInfo(pclsCache, UserId);
                if (BaseList != null)
                {
                    if (BaseList.Birthday != "" && BaseList.Birthday != "0" && BaseList.Birthday != null)
                    {
                        Input.Age = new UsersMethod().GetAgeByBirthDay(pclsCache, Convert.ToInt32(BaseList.Birthday));//年龄
                    }
                    if (BaseList.Gender != "" && BaseList.Gender != "0" && BaseList.Gender != null)
                    {
                        Input.Gender = Convert.ToInt32(BaseList.Gender);//性别
                    }
                }
            }

            return Input;
        }

        /// <summary>
        /// 获取高血压风险评估结果 SYF 2015-11-16
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public M1Risk GetM1Risk(DataConnection pclsCache, string UserId)
        {
            M1Risk Output = new M1Risk();
            M1RiskInput Input = new M1RiskInput();
            Input = new RiskInfoRepository().GetM1RiskInput(pclsCache, UserId);
            if (Input != null)
            {
                #region//获取模型所需输入
                int Age = Input.Age;//年龄默认1岁（避免出现0岁）
                int Gender = Input.Gender;//性别男
                int Height = Input.Height;//身高176cm
                int Weight = Input.Weight;//体重69千克
                double BMI = Input.BMI;
                int AbdominalGirth = Input.AbdominalGirth; //腹围
                int Heartrate = Input.Heartrate;//心率
                int Parent = Input.Parent;//父母中至少有一方有高血压
                int Smoke = Input.Smoke;//不抽烟
                int Stroke = Input.Stroke;//没有中风
                int Lvh = Input.Lvh; ;//有左心室肥大
                int Diabetes = Input.Diabetes;//有伴随糖尿病
                int Treat = Input.Treat;//高血压是否在治疗（接受过）没有
                int Heartattack = Input.Heartattack;//有过心脏事件（心血管疾病）
                int Af = Input.Af;//没有过房颤
                int Chd = Input.Chd;//有冠心病(心肌梗塞)
                int Valve = Input.Valve;//没有心脏瓣膜病
                double Tcho = Input.Tcho;//总胆固醇浓度5.2mmol/L
                double Creatinine = Input.Creatinine;//肌酐浓度140μmoI/L
                double Hdlc = Input.Hdlc;//高密度脂蛋白胆固醇1.21g/ml
                int SBP = Input.SBP;//当前收缩压
                int DBP = Input.DBP;//当前舒张压
                #endregion

                #region//用于模型计算时的调整
                if (Gender <= 2)
                {
                    Gender = Gender - 1;
                }
                else
                {
                    Gender = 0;
                }
                if (Gender == 1)//为计算方便，性别值对调
                {
                    Gender = 0;
                }
                else
                {
                    Gender = 1;
                }
                if (Parent > 1)
                {
                    Parent = 0;
                }
                if (Smoke > 1)
                {
                    Smoke = 0;
                }
                if (Stroke > 1)
                {
                    Stroke = 0;
                }
                if (Lvh > 1)
                {
                    Lvh = 0;
                }
                if (Diabetes > 1)
                {
                    Diabetes = 0;
                }
                if (Treat > 1)
                {
                    Treat = 0;
                }
                if (Heartattack > 1)
                {
                    Heartattack = 0;
                }
                if (Af > 1)
                {
                    Af = 0;
                }
                if (Chd > 1)
                {
                    Chd = 0;
                }
                if (Valve > 1)
                {
                    Valve = 0;
                }
                #endregion

                #region//高血压风险
                double Hyperother = -0.15641 * Age - 0.20293 * Gender - 0.19073 * Smoke - 0.16612 * Parent - 0.03388 * BMI;
                Hyperother = Hyperother - 0.05933 * SBP - 0.12847 * DBP + 0.00162 * Age * DBP;
                double Hyper = 1 - Math.Exp(-Math.Exp(((Math.Log(4)) - (22.94954 + Hyperother)) / 0.87692));
                #endregion

                #region//HarvardRiskInfactor这个变量存的是Harvard风险评估计算公式中的风险因数，界面上需要做的是加上收缩压的风险因数，然后代入公式计算。
                int HarvardRiskInfactor = 0;

                //男性风险值
                if (Gender == 1)
                {
                    if (Age <= 39)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 19;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 0;
                        }
                    }
                    else if (Age <= 44 && Age >= 40)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 7;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 4;
                        }
                    }
                    else if (Age <= 49 && Age >= 45)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 7;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 7;
                        }
                    }
                    else if (Age <= 54 && Age >= 50)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 11;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 6;
                        }
                    }
                    else if (Age <= 59 && Age >= 55)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 14;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 6;
                        }
                    }
                    else if (Age <= 64 && Age >= 60)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 18;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 5;
                        }
                    }
                    else if (Age <= 69 && Age >= 65)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 22;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 4;
                        }
                    }
                    else
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 25;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 4;
                        }
                    }
                    //年龄和抽烟的风险值加成
                    if (Tcho < 5)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 0;
                    }
                    else if (Tcho >= 5.0 && Tcho <= 5.9)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 2;
                    }
                    else if (Tcho >= 6.0 && Tcho <= 6.9)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 4;
                    }
                    else if (Tcho >= 7.0 && Tcho <= 7.9)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 5;
                    }
                    else if (Tcho >= 8.0 && Tcho <= 8.9)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 7;
                    }
                    else
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 9;
                    }
                    //总胆固醇浓度风险值加成
                    if (Height < 145)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 6;
                    }
                    else if (Height >= 145 && Height <= 154)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 4;
                    }
                    else if (Height >= 155 && Height <= 164)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 3;
                    }
                    else if (Height >= 165 && Height <= 174)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 2;
                    }
                    else
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 0;
                    }
                    //身高风险值加成
                    if (Creatinine < 50)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 0;
                    }
                    else if (Creatinine >= 50 && Creatinine <= 69)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 1;
                    }
                    else if (Creatinine >= 70 && Creatinine <= 89)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 2;
                    }
                    else if (Creatinine >= 90 && Creatinine <= 109)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 3;
                    }
                    else
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 4;
                    }
                    //肌酐浓度风险值加成
                    if (Chd == 1)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 8;
                    }
                    //心肌梗塞（冠心病）风险值加成
                    if (Stroke == 1)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 8;
                    }
                    //中风风险值加成 
                    if (Lvh == 1)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 3;
                    }
                    //左室高血压（左心室肥大）风险值加成
                    if (Diabetes == 1)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 2;
                    }
                    //糖尿病风险值加成
                    if (SBP <= 119)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 0;
                    }
                    else if (SBP >= 120 && SBP <= 129)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 1;
                    }
                    else if (SBP >= 130 && SBP <= 139)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 2;
                    }
                    else if (SBP >= 130 && SBP <= 139)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 2;
                    }
                    else if (SBP >= 140 && SBP <= 149)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 3;
                    }
                    else if (SBP >= 150 && SBP <= 159)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 4;
                    }
                    else if (SBP >= 160 && SBP <= 169)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 5;
                    }
                    else if (SBP >= 170 && SBP <= 179)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 6;
                    }
                    else if (SBP >= 180 && SBP <= 189)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 8;
                    }
                    else if (SBP >= 190 && SBP <= 199)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 9;
                    }
                    else if (SBP >= 200 && SBP <= 209)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 10;
                    }
                    else
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 11;
                    }
                    //收缩压加成
                }
                //女性风险值计算
                else
                {
                    if (Age <= 39)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 13;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 0;
                        }
                    }
                    else if (Age <= 44 && Age >= 40)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 12;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 5;
                        }
                    }
                    else if (Age <= 49 && Age >= 45)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 11;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 9;
                        }
                    }
                    else if (Age <= 54 && Age >= 50)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 10;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 14;
                        }
                    }
                    else if (Age <= 59 && Age >= 55)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 10;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 18;
                        }
                    }
                    else if (Age <= 64 && Age >= 60)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 9;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 23;
                        }
                    }
                    else if (Age <= 69 && Age >= 65)
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 9;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 27;
                        }
                    }
                    else
                    {
                        if (Smoke == 1)
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 8;
                        }
                        else
                        {
                            HarvardRiskInfactor = HarvardRiskInfactor + 32;
                        }
                    }
                    //年龄和抽烟的风险值加成
                    if (Tcho < 5)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 0;
                    }
                    else if (Tcho >= 5.0 && Tcho <= 5.9)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 0;
                    }
                    else if (Tcho >= 6.0 && Tcho <= 6.9)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 1;
                    }
                    else if (Tcho >= 7.0 && Tcho <= 7.9)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 1;
                    }
                    else if (Tcho >= 8.0 && Tcho <= 8.9)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 2;
                    }
                    else
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 2;
                    }
                    //总胆固醇浓度风险值加成
                    if (Height < 145)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 6;
                    }
                    else if (Height >= 145 && Height <= 154)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 4;
                    }
                    else if (Height >= 155 && Height <= 164)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 3;
                    }
                    else if (Height >= 165 && Height <= 174)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 2;
                    }
                    else
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 0;
                    }
                    //身高风险值加成
                    if (Creatinine < 50)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 0;
                    }
                    else if (Creatinine >= 50 && Creatinine <= 69)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 1;
                    }
                    else if (Creatinine >= 70 && Creatinine <= 89)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 2;
                    }
                    else if (Creatinine >= 90 && Creatinine <= 109)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 3;
                    }
                    else
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 4;
                    }
                    //肌酐浓度风险值加成
                    if (Chd == 1)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 8;
                    }
                    //心肌梗塞（冠心病）风险值加成
                    if (Stroke == 1)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 8;
                    }
                    //中风风险值加成 
                    if (Lvh == 1)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 3;
                    }
                    //左室高血压（左心室肥大）风险值加成
                    if (Diabetes == 1)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 9;
                    }
                    //糖尿病风险值加成
                    if (SBP <= 119)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 0;
                    }
                    else if (SBP >= 120 && SBP <= 129)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 1;
                    }
                    else if (SBP >= 130 && SBP <= 139)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 2;
                    }
                    else if (SBP >= 130 && SBP <= 139)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 2;
                    }
                    else if (SBP >= 140 && SBP <= 149)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 3;
                    }
                    else if (SBP >= 150 && SBP <= 159)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 4;
                    }
                    else if (SBP >= 160 && SBP <= 169)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 5;

                    }
                    else if (SBP >= 170 && SBP <= 179)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 6;
                    }
                    else if (SBP >= 180 && SBP <= 189)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 7;
                    }
                    else if (SBP >= 190 && SBP <= 199)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 8;
                    }
                    else if (SBP >= 200 && SBP <= 209)
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 9;
                    }
                    else
                    {
                        HarvardRiskInfactor = HarvardRiskInfactor + 10;
                    }
                    //收缩压加成
                }
                #endregion//以上是女性风险值
                double Harvard = 6.304 * Math.Pow(10, -8) * Math.Pow(HarvardRiskInfactor, 5) - 5.027 * Math.Pow(10, -6) * Math.Pow(HarvardRiskInfactor, 4) + 0.0001768 * Math.Pow(HarvardRiskInfactor, 3) - 0.001998 * Math.Pow(HarvardRiskInfactor, 2) + 0.01294 * HarvardRiskInfactor + 0.0409;
                Harvard = Harvard / 100;

                #region//FraminghamRiskInfactor这个变量存的是Framingham风险评估计算公式中的风险因数，界面上需要做的是加上收缩压的风险因数，然后代入公式计算。
                //这个Framingham模型也是需要收缩压值的，分为接受过治疗的血压和未接受过治疗的血压，模型分为男女进行计算，因为不同性别公式不同
                double FraminghamRiskInfactor = 0.0;
                double Framingham = 0.0;
                if (Gender == 1) //男性
                {
                    FraminghamRiskInfactor = FraminghamRiskInfactor + Math.Log(Age) * 3.06117;//性别
                    FraminghamRiskInfactor = FraminghamRiskInfactor + Math.Log(Tcho) * 1.12370;//总胆固醇
                    FraminghamRiskInfactor = FraminghamRiskInfactor + Math.Log(Hdlc) * (-0.93263);//高密度脂蛋白胆固醇
                    if (Smoke == 1)
                    {
                        FraminghamRiskInfactor = FraminghamRiskInfactor + 0.65451;//抽烟
                    }
                    if (Diabetes == 1)
                    {
                        FraminghamRiskInfactor = FraminghamRiskInfactor + 0.57367;//抽烟
                    }
                    if (Treat == 1)
                    {
                        FraminghamRiskInfactor = FraminghamRiskInfactor + 1.99881 * Math.Log(SBP);
                    }
                    else
                    {
                        FraminghamRiskInfactor = FraminghamRiskInfactor + 1.93303 * Math.Log(SBP);
                    }
                    Framingham = 1 - Math.Pow(0.88936, Math.Exp(FraminghamRiskInfactor - 23.9802));
                }
                else //女性
                {
                    FraminghamRiskInfactor = FraminghamRiskInfactor + Math.Log(Age) * 2.3288;//性别
                    FraminghamRiskInfactor = FraminghamRiskInfactor + Math.Log(Tcho) * 1.20904;//总胆固醇
                    FraminghamRiskInfactor = FraminghamRiskInfactor + Math.Log(Hdlc) * (-0.70833);//高密度脂蛋白胆固醇
                    if (Smoke == 1)
                    {
                        FraminghamRiskInfactor = FraminghamRiskInfactor + 0.52873;//抽烟
                    }
                    if (Diabetes == 1)
                    {
                        FraminghamRiskInfactor = FraminghamRiskInfactor + 0.69154;//抽烟
                    }
                    if (Treat == 1)
                    {
                        FraminghamRiskInfactor = FraminghamRiskInfactor + 2.82263 * Math.Log(SBP);
                    }
                    else
                    {
                        FraminghamRiskInfactor = FraminghamRiskInfactor + 2.76157 * Math.Log(SBP);
                    }
                    Framingham = 1 - Math.Pow(0.95012, Math.Exp(FraminghamRiskInfactor - 26.1931));
                }
                #endregion

                #region//StrokeRiskInfactor这个变量存的是中风风险评估计算公式中的风险因数，界面上需要做的是加上收缩压的风险因数，然后计算。
                int StrokeRiskInfactor = 0;
                double StrokeRisk = 0.0;
                if (Gender == 1) //男性
                {
                    if (Age <= 56)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 0;
                    }
                    else if (Age >= 57 && Age <= 59)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 1;
                    }
                    else if (Age >= 60 && Age <= 62)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 2;
                    }
                    else if (Age >= 63 && Age <= 65)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 3;
                    }
                    else if (Age >= 66 && Age <= 68)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 4;
                    }
                    else if (Age >= 69 && Age <= 72)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 5;
                    }
                    else if (Age >= 73 && Age <= 75)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 6;
                    }
                    else if (Age >= 76 && Age <= 78)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 7;
                    }
                    else if (Age >= 79 && Age <= 81)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 8;
                    }
                    else if (Age >= 82 && Age <= 84)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 9;
                    }
                    else
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 10;
                    }
                    if (Diabetes == 1)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 2;
                    }
                    //糖尿病风险值加成
                    if (Smoke == 1)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 3;
                    }
                    //吸烟风险值加成
                    if (Heartattack == 1)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 4;
                    }
                    //心血管疾病史（心脏事件）风险值加成
                    if (Af == 1)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 4;
                    }
                    //房颤风险值加成
                    if (Lvh == 1)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 5;
                    }
                    if (Treat != 1) //没有治疗过高血压的情况
                    {
                        if (SBP <= 105)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 0;
                        }
                        else if (SBP >= 106 && SBP <= 115)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 1;
                        }
                        else if (SBP >= 116 && SBP <= 125)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 2;
                        }
                        else if (SBP >= 126 && SBP <= 135)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 3;
                        }
                        else if (SBP >= 136 && SBP <= 145)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 4;
                        }
                        else if (SBP >= 146 && SBP <= 155)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 5;
                        }
                        else if (SBP >= 156 && SBP <= 165)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 6;
                        }
                        else if (SBP >= 166 && SBP <= 175)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 7;
                        }
                        else if (SBP >= 176 && SBP <= 185)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 8;
                        }
                        else if (SBP >= 186 && SBP <= 195)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 9;
                        }
                        else
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 10;
                        }
                    }
                    else//治疗过高血压的情况
                    {
                        if (SBP <= 105)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 0;
                        }
                        else if (SBP >= 106 && SBP <= 112)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 1;
                        }
                        else if (SBP >= 113 && SBP <= 117)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 2;
                        }
                        else if (SBP >= 118 && SBP <= 123)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 3;
                        }
                        else if (SBP >= 124 && SBP <= 129)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 4;
                        }
                        else if (SBP >= 130 && SBP <= 135)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 5;
                        }
                        else if (SBP >= 136 && SBP <= 142)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 6;
                        }
                        else if (SBP >= 143 && SBP <= 150)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 7;
                        }
                        else if (SBP >= 151 && SBP <= 161)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 8;
                        }
                        else if (SBP >= 162 && SBP <= 176)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 9;
                        }
                        else
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 10;
                        }
                    }
                    double[] Risk = new double[] { 3, 3, 4, 4, 5, 5, 6, 7, 8, 10, 11, 13, 15, 17, 20, 22, 26, 29, 33, 37, 42, 47, 52, 57, 63, 68, 74, 79, 84, 88 };
                    StrokeRisk = Risk[StrokeRiskInfactor - 1] / 100;
                }
                else //女性
                {
                    if (Age <= 56)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 0;
                    }
                    else if (Age >= 57 && Age <= 59)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 1;
                    }
                    else if (Age >= 60 && Age <= 62)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 2;
                    }
                    else if (Age >= 63 && Age <= 64)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 3;
                    }
                    else if (Age >= 65 && Age <= 67)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 4;
                    }
                    else if (Age >= 68 && Age <= 70)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 5;
                    }
                    else if (Age >= 71 && Age <= 73)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 6;
                    }
                    else if (Age >= 74 && Age <= 76)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 7;
                    }
                    else if (Age >= 77 && Age <= 78)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 8;
                    }
                    else if (Age >= 79 && Age <= 81)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 9;
                    }
                    else
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 10;
                    }
                    if (Diabetes == 1)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 3;
                    }
                    //糖尿病风险值加成
                    if (Smoke == 1)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 3;
                    }
                    //吸烟风险值加成
                    if (Heartattack == 1)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 2;
                    }
                    //心血管疾病史（心脏事件）风险值加成
                    if (Af == 1)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 6;
                    }
                    //房颤风险值加成
                    if (Lvh == 1)
                    {
                        StrokeRiskInfactor = StrokeRiskInfactor + 4;
                    }
                    if (Treat != 1) //没有治疗过高血压的情况
                    {
                        if (SBP <= 94)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 0;
                        }
                        else if (SBP >= 95 && SBP <= 106)
                        {


                            StrokeRiskInfactor = StrokeRiskInfactor + 1;
                        }
                        else if (SBP >= 107 && SBP <= 118)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 2;
                        }
                        else if (SBP >= 119 && SBP <= 130)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 3;
                        }
                        else if (SBP >= 131 && SBP <= 143)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 4;
                        }
                        else if (SBP >= 144 && SBP <= 155)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 5;
                        }
                        else if (SBP >= 156 && SBP <= 167)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 6;
                        }
                        else if (SBP >= 168 && SBP <= 180)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 7;
                        }
                        else if (SBP >= 181 && SBP <= 192)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 8;
                        }
                        else if (SBP >= 193 && SBP <= 204)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 9;
                        }
                        else
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 10;
                        }
                    }
                    else//治疗过高血压的情况
                    {
                        if (SBP <= 94)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 0;
                        }
                        else if (SBP >= 95 && SBP <= 106)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 1;
                        }
                        else if (SBP >= 107 && SBP <= 113)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 2;
                        }
                        else if (SBP >= 114 && SBP <= 119)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 3;
                        }
                        else if (SBP >= 120 && SBP <= 125)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 4;
                        }
                        else if (SBP >= 126 && SBP <= 131)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 5;
                        }
                        else if (SBP >= 132 && SBP <= 139)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 6;
                        }
                        else if (SBP >= 140 && SBP <= 148)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 7;
                        }
                        else if (SBP >= 149 && SBP <= 160)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 8;
                        }
                        else if (SBP >= 161 && SBP <= 204)
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 9;
                        }
                        else
                        {
                            StrokeRiskInfactor = StrokeRiskInfactor + 10;
                        }
                    }
                    double[] Risk = new double[] { 1, 1, 2, 2, 2, 3, 4, 4, 5, 6, 8, 9, 11, 13, 16, 19, 23, 27, 32, 37, 43, 50, 57, 64, 71, 78, 84 };
                    StrokeRisk = Risk[StrokeRiskInfactor - 1] / 100;
                }
                #endregion

                #region//HeartFailureRiskInfactor这个变量存的是心衰风险评估计算公式中的风险因数，界面上需要做的是加上收缩压的风险因数，然后计算。
                int HeartFailureRiskInfactor = 0;
                double HeartFailureRisk = 0.0;
                if (Gender == 1) //男性
                {
                    if (Age <= 49)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 0;
                    }
                    else if (Age >= 50 && Age <= 54)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 1;
                    }
                    else if (Age >= 55 && Age <= 59)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 2;
                    }
                    else if (Age >= 60 && Age <= 64)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 3;
                    }
                    else if (Age >= 65 && Age <= 69)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 4;
                    }
                    else if (Age >= 70 && Age <= 74)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 5;
                    }
                    else if (Age >= 75 && Age <= 79)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 6;
                    }
                    else if (Age >= 80 && Age <= 84)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 7;
                    }
                    else if (Age >= 85 && Age <= 89)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 8;
                    }
                    else
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 9;
                    }
                    if (Heartrate <= 54)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 0;
                    }
                    else if (Heartrate >= 55 && Heartrate <= 64)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 1;
                    }
                    else if (Heartrate >= 65 && Heartrate <= 79)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 2;
                    }
                    else if (Heartrate >= 80 && Heartrate <= 89)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 3;
                    }
                    else if (Heartrate >= 90 && Heartrate <= 104)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 4;
                    }
                    else
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 5;
                    }
                    //心率风险值加成
                    if (Lvh == 1)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 4;
                    }
                    //左心室肥大（左室高血压）风险值加成
                    if (Chd == 1)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 8;
                    }
                    //冠心病风险值加成
                    if (Valve == 1)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 5;
                    }
                    //瓣膜疾病风险值加成
                    if (Smoke == 1)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 1;
                    }
                    //糖尿病风险值加成
                    if (SBP <= 119)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 0;
                    }
                    else if (SBP >= 120 && SBP <= 139)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 1;
                    }
                    else if (SBP >= 140 && SBP <= 169)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 2;
                    }
                    else if (SBP >= 170 && SBP <= 189)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 3;
                    }
                    else if (SBP >= 190 && SBP <= 219)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 4;
                    }
                    else
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 5;
                    }

                    if (HeartFailureRiskInfactor <= 5)
                    {
                        HeartFailureRisk = 1;
                    }
                    else if (HeartFailureRiskInfactor > 5 && HeartFailureRiskInfactor < 14)
                    {
                        HeartFailureRisk = 3;
                    }
                    else if (HeartFailureRiskInfactor >= 14 && HeartFailureRiskInfactor < 16)
                    {
                        HeartFailureRisk = 5;
                    }
                    else if (HeartFailureRiskInfactor >= 16 && HeartFailureRiskInfactor < 18)
                    {
                        HeartFailureRisk = 8;
                    }
                    else if (HeartFailureRiskInfactor >= 18 && HeartFailureRiskInfactor < 20)
                    {
                        HeartFailureRisk = 11;
                    }
                    else if (HeartFailureRiskInfactor >= 20 && HeartFailureRiskInfactor < 22)
                    {
                        HeartFailureRisk = 11;
                    }
                    else if (HeartFailureRiskInfactor >= 22 && HeartFailureRiskInfactor < 24)
                    {
                        HeartFailureRisk = 22;
                    }
                    else if (HeartFailureRiskInfactor >= 24 && HeartFailureRiskInfactor < 25)
                    {
                        HeartFailureRisk = 30;
                    }
                    else if (HeartFailureRiskInfactor >= 25 && HeartFailureRiskInfactor < 26)
                    {
                        HeartFailureRisk = 34;
                    }
                    else if (HeartFailureRiskInfactor >= 26 && HeartFailureRiskInfactor < 27)
                    {
                        HeartFailureRisk = 39;
                    }
                    else if (HeartFailureRiskInfactor >= 27 && HeartFailureRiskInfactor < 28)
                    {
                        HeartFailureRisk = 44;
                    }
                    else if (HeartFailureRiskInfactor >= 28 && HeartFailureRiskInfactor < 29)
                    {
                        HeartFailureRisk = 49;
                    }
                    else if (HeartFailureRiskInfactor >= 29 && HeartFailureRiskInfactor < 30)
                    {
                        HeartFailureRisk = 54;
                    }
                    else
                    {
                        HeartFailureRisk = 59;
                    }

                    HeartFailureRisk = HeartFailureRisk / 100;
                }
                else //女性
                {
                    if (Age <= 49)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 0;
                    }
                    else if (Age >= 50 && Age <= 54)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 1;
                    }
                    else if (Age >= 55 && Age <= 59)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 2;
                    }
                    else if (Age >= 60 && Age <= 64)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 3;
                    }
                    else if (Age >= 65 && Age <= 69)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 4;
                    }
                    else if (Age >= 70 && Age <= 74)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 5;
                    }
                    else if (Age >= 75 && Age <= 79)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 6;
                    }
                    else if (Age >= 80 && Age <= 84)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 7;
                    }
                    else if (Age >= 85 && Age <= 89)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 8;
                    }
                    else
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 9;
                    }
                    //年龄的风险加权值
                    if (Heartrate < 60)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 0;
                    }
                    else if (Heartrate >= 60 && Heartrate <= 79)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 1;
                    }
                    else if (Heartrate >= 80 && Heartrate <= 104)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 2;
                    }
                    else
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 3;
                    }
                    //心率风险值加成
                    if (Lvh == 1)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 5;
                    }
                    //左心室肥大（左室高血压）风险值加成
                    if (Chd == 1)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 6;
                    }
                    //冠心病风险值加成
                    if (Valve == 1)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 6;
                        if (Smoke == 1)
                        {
                            HeartFailureRiskInfactor = HeartFailureRiskInfactor + 2;
                        }
                    }
                    else
                    {
                        if (Smoke == 1)
                        {
                            HeartFailureRiskInfactor = HeartFailureRiskInfactor + 6;
                        }
                    }
                    //瓣膜疾病和糖尿病风险值加成
                    if (BMI < 21)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 0;
                    }
                    else if (BMI >= 21 && BMI <= 25)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 1;
                    }
                    else if (BMI > 25 && BMI <= 29)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 2;
                    }
                    else
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 3;
                    }
                    if (SBP < 140)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 0;
                    }
                    else if (SBP >= 140 && SBP <= 209)
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 1;
                    }
                    else
                    {
                        HeartFailureRiskInfactor = HeartFailureRiskInfactor + 2;
                    }

                    if (HeartFailureRiskInfactor < 10)
                    {
                        HeartFailureRisk = 1;
                    }
                    else if (HeartFailureRiskInfactor <= 28)
                    {
                        double[] Risk = new double[] { 2, 2, 3, 3, 4, 5, 7, 9, 11, 14, 17, 21, 25, 30, 36, 42, 48, 54, 60 };
                        HeartFailureRisk = Risk[HeartFailureRiskInfactor - 10];
                    }
                    else
                    {
                        HeartFailureRisk = 60;
                    }
                    HeartFailureRisk = HeartFailureRisk / 100;
                }
                #endregion

                #region//输出数据
                Output.Hyper = Math.Round(Hyper * 100, 2);
                Output.Harvard = Math.Round(Harvard * 100, 2);
                Output.Framingham = Math.Round(Framingham * 100, 2);
                Output.StrokeRisk = Math.Round(StrokeRisk * 100, 2);
                Output.HeartFailureRisk = Math.Round(HeartFailureRisk * 100, 2);
                Output.SBP = SBP;
                Output.DBP = DBP;
                #endregion
            }
            else 
            {
                Output = null;
            }
            return Output;
        }

        public M1Risk AddM1Risk(DataConnection pclsCache, string PatientId, M1RiskInput M1RiskInput, int RecordDate, int RecordTime, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            M1Risk M1Risk = new M1Risk();
            int ret = 0;
            ret = new RiskInfoMethod().SetM1RiskInput(pclsCache, PatientId, M1RiskInput, RecordDate, RecordTime, piUserId, piTerminalName, piTerminalIP, piDeviceType);
            if (ret == 1)
            {
                M1Risk = new RiskInfoRepository().GetM1Risk(pclsCache, PatientId);
            }
            return M1Risk;
        }

        /// <summary>
        /// 获取心衰模型评估的所有输入 SYF 20151117
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public M3RiskInput GetM3RiskInput(DataConnection pclsCache, string UserId)
        {
            M3RiskInput Input = new M3RiskInput();
            Input = new UsersMethod().GetM3RiskInput(pclsCache, UserId);
            if (Input != null)
            {
                BasicInfo BaseList = new UsersMethod().GetBasicInfo(pclsCache, UserId);
                if (BaseList != null)
                {
                    if (BaseList.Birthday != "" && BaseList.Birthday != "0" && BaseList.Birthday != null)
                    {
                        Input.Age = new UsersMethod().GetAgeByBirthDay(pclsCache, Convert.ToInt32(BaseList.Birthday));//年龄
                    }
                    if (BaseList.Gender != "" && BaseList.Gender != "0" && BaseList.Gender != null)
                    {
                        Input.Gender = Convert.ToInt32(BaseList.Gender);//性别
                    }
                }
            }
            else
            {
                return null;
            }
            return Input;
        }

        /// <summary>
        /// 获取心衰风险评估结果 SYF 2015-11-17
        /// </summary>
        /// <param name="pclsCache"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public M3Risk GetM3Risk(DataConnection pclsCache, string UserId)
        {
            M3RiskInput Input = new M3RiskInput();
            M3Risk Output = new M3Risk();
            Input = new RiskInfoRepository().GetM3RiskInput(pclsCache, UserId);
            if (Input != null)
            {
                
                int RiskScore = 0;
                #region//左心室射血分数影响
                if (Input.EF >= 40)
                {
                    RiskScore = RiskScore + 0;
                }
                else if (Input.EF >= 35 && Input.EF <= 39)
                {
                    RiskScore = RiskScore + 2;
                }
                else if (Input.EF >= 30 && Input.EF <= 34)
                {
                    RiskScore = RiskScore + 3;
                }
                else if (Input.EF >= 25 && Input.EF <= 29)
                {
                    RiskScore = RiskScore + 5;
                }
                else if (Input.EF >= 20 && Input.EF <= 24)
                {
                    RiskScore = RiskScore + 6;
                }
                else if (Input.EF >= 0 && Input.EF < 20)
                {
                    RiskScore = RiskScore + 7;
                }
                #endregion

                #region//年龄与左心室射血分数叠加影响
                if ((Input.Age >= 56) && (Input.Age <= 59))
                {
                    if (Input.EF < 30)
                    {
                        RiskScore = RiskScore + 1;
                    }
                    else if ((Input.EF >= 30) && (Input.EF <= 39))
                    {
                        RiskScore = RiskScore + 2;
                    }
                    else if (Input.EF >= 40)
                    {
                        RiskScore = RiskScore + 3;
                    }
                }
                else if ((Input.Age >= 60) && (Input.Age <= 64))
                {
                    if (Input.EF < 30)
                    {
                        RiskScore = RiskScore + 2;
                    }
                    else if ((Input.EF >= 30) && (Input.EF <= 39))
                    {
                        RiskScore = RiskScore + 4;
                    }
                    else if (Input.EF >= 40)
                    {
                        RiskScore = RiskScore + 5;
                    }
                }
                else if ((Input.Age >= 65) && (Input.Age <= 69))
                {
                    if (Input.EF < 30)
                    {
                        RiskScore = RiskScore + 4;
                    }
                    else if ((Input.EF >= 30) && (Input.EF <= 39))
                    {
                        RiskScore = RiskScore + 6;
                    }
                    else if (Input.EF >= 40)
                    {
                        RiskScore = RiskScore + 7;
                    }
                }
                else if ((Input.Age >= 70) && (Input.Age <= 74))
                {
                    if (Input.EF < 30)
                    {
                        RiskScore = RiskScore + 6;
                    }
                    else if ((Input.EF >= 30) && (Input.EF <= 39))
                    {
                        RiskScore = RiskScore + 8;
                    }
                    else if (Input.EF >= 40)
                    {
                        RiskScore = RiskScore + 9;
                    }
                }
                else if ((Input.Age >= 75) && (Input.Age <= 79))
                {
                    if (Input.EF < 30)
                    {
                        RiskScore = RiskScore + 8;
                    }
                    else if ((Input.EF >= 30) && (Input.EF <= 39))
                    {
                        RiskScore = RiskScore + 10;
                    }
                    else if (Input.EF >= 40)
                    {
                        RiskScore = RiskScore + 12;
                    }
                }
                else if (Input.Age >= 80)
                {
                    if (Input.EF < 30)
                    {
                        RiskScore = RiskScore + 10;
                    }
                    else if ((Input.EF >= 30) && (Input.EF <= 39))
                    {
                        RiskScore = RiskScore + 13;
                    }
                    else if (Input.EF >= 40)
                    {
                        RiskScore = RiskScore + 15;
                    }
                }
                #endregion

                #region//收缩压与左心室射血分数叠加影响
                if (Input.SBP < 110)
                {
                    if (Input.EF < 30)
                    {
                        RiskScore = RiskScore + 5;
                    }
                    else if ((Input.EF >= 30) && (Input.EF <= 39))
                    {
                        RiskScore = RiskScore + 3;
                    }
                    else if (Input.EF >= 40)
                    {
                        RiskScore = RiskScore + 2;
                    }
                }
                else if ((Input.SBP >= 110) && (Input.SBP <= 119))
                {
                    if (Input.EF < 30)
                    {
                        RiskScore = RiskScore + 4;
                    }
                    else if ((Input.EF >= 30) && (Input.EF <= 39))
                    {
                        RiskScore = RiskScore + 2;
                    }
                    else if (Input.EF >= 40)
                    {
                        RiskScore = RiskScore + 1;
                    }
                }
                else if ((Input.SBP >= 120) && (Input.SBP <= 129))
                {
                    if (Input.EF < 30)
                    {
                        RiskScore = RiskScore + 3;
                    }
                    else if ((Input.EF >= 30) && (Input.EF <= 39))
                    {
                        RiskScore = RiskScore + 1;
                    }
                    else if (Input.EF >= 40)
                    {
                        RiskScore = RiskScore + 1;
                    }
                }
                else if ((Input.SBP >= 130) && (Input.SBP <= 139))
                {
                    if (Input.EF < 30)
                    {
                        RiskScore = RiskScore + 2;
                    }
                    else if ((Input.EF >= 30) && (Input.EF <= 39))
                    {
                        RiskScore = RiskScore + 1;
                    }
                }
                else if ((Input.SBP >= 140) && (Input.SBP <= 149))
                {
                    if (Input.EF < 30)
                    {
                        RiskScore = RiskScore + 1;
                    }
                }
                #endregion

                #region//BMI的影响
                if (Input.BMI < 15)
                {
                    RiskScore = RiskScore + 6;
                }
                else if ((Input.BMI >= 15) && (Input.BMI < 20))
                {
                    RiskScore = RiskScore + 5;
                }
                else if ((Input.BMI >= 20) && (Input.BMI < 25))
                {
                    RiskScore = RiskScore + 3;
                }
                else if ((Input.BMI >= 25) && (Input.BMI < 30))
                {
                    RiskScore = RiskScore + 2;
                }
                #endregion

                #region//肌酐的影响
                if ((Input.Creatinine >= 90) && (Input.Creatinine < 110))
                {
                    RiskScore = RiskScore + 1;
                }
                else if ((Input.Creatinine >= 110) && (Input.Creatinine < 130))
                {
                    RiskScore = RiskScore + 2;
                }
                else if ((Input.Creatinine >= 130) && (Input.Creatinine < 150))
                {
                    RiskScore = RiskScore + 3;
                }
                else if ((Input.Creatinine >= 150) && (Input.Creatinine < 170))
                {
                    RiskScore = RiskScore + 4;
                }
                else if ((Input.Creatinine >= 170) && (Input.Creatinine < 210))
                {
                    RiskScore = RiskScore + 5;
                }
                else if ((Input.Creatinine >= 210) && (Input.Creatinine < 250))
                {
                    RiskScore = RiskScore + 6;
                }
                else if (Input.Creatinine >= 250)
                {
                    RiskScore = RiskScore + 8;
                }
                #endregion

                #region//心衰等级分类影响
                if (Input.NYHA == 2)
                {
                    RiskScore = RiskScore + 2;
                }
                else if (Input.NYHA == 3)
                {
                    RiskScore = RiskScore + 6;
                }
                else if (Input.NYHA == 4)
                {
                    RiskScore = RiskScore + 8;
                }
                #endregion

                #region//其他因素影响
                if (Input.Gender == 1)
                {
                    RiskScore = RiskScore + 1;
                }
                if (Input.Smoke == 1)
                {
                    RiskScore = RiskScore + 1;
                }
                if (Input.Diabetes == 1)
                {
                    RiskScore = RiskScore + 3;
                }
                if (Input.Lung == 1)
                {
                    RiskScore = RiskScore + 2;
                }
                if (Input.HF18 == 1)
                {
                    RiskScore = RiskScore + 2;
                }
                if (Input.Beta == 2)
                {
                    RiskScore = RiskScore + 3;
                }
                if (Input.AA == 2)
                {
                    RiskScore = RiskScore + 1;
                }
                #endregion

                double[] OneYear = new double[] { 0.015, 0.016, 0.018, 0.020, 0.022, 0.024, 0.027, 0.029, 0.032, 0.036, 0.039, 0.043, 0.048, 0.052, 0.058, 0.063, 0.070, 0.077, 0.084, 0.093, 0.102, 0.111, 0.122, 0.134, 0.147, 0.060, 0.175, 0.191, 0.209, 0.227, 0.248, 0.269, 0.292, 0.316, 0.342, 0.369, 0.398, 0.427, 0.458, 0.490, 0.523, 0.557, 0.591, 0.625, 0.659, 0.692, 0.725, 0.757, 0.787, 0.816, 0.842 };
                double[] ThreeYear = new double[] { 0.039, 0.043, 0.048, 0.052, 0.058, 0.063, 0.070, 0.077, 0.084, 0.092, 0.102, 0.111, 0.122, 0.134, 0.146, 0.160, 0.175, 0.191, 0.209, 0.227, 0.247, 0.269, 0.292, 0.316, 0.342, 0.369, 0.397, 0.427, 0.458, 0.490, 0.523, 0.556, 0.590, 0.625, 0.658, 0.692, 0.725, 0.756, 0.787, 0.815, 0.842, 0.866, 0.889, 0.908, 0.926, 0.941, 0.953, 0.964, 0.973, 0.980, 0.985 };
                Output.One = OneYear[RiskScore] * 100;
                Output.Three = ThreeYear[RiskScore] * 100;
            }
            else
            {
                return null;
            }
            return Output;
        }

        public M3Risk AddM3Risk(DataConnection pclsCache, string PatientId, M3RiskInput M3RiskInput, int RecordDate, int RecordTime, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            M3Risk M3Risk = new M3Risk();
            int ret = 0;
            ret = new RiskInfoMethod().SetM3RiskInput(pclsCache, PatientId, M3RiskInput, RecordDate, RecordTime, piUserId, piTerminalName, piTerminalIP, piDeviceType);
            if (ret == 1)
            {
                M3Risk = new RiskInfoRepository().GetM3Risk(pclsCache, PatientId);
            }
            return M3Risk;
        }
    }
}