using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CDMISrestful.DataModels
{
    public class Users
    {
    }

    public class ModulesByPID
    {
        public string CategoryCode { get; set; }
        public string Modules { get; set; }

        public string DoctorId { get; set; }

    }

    public class ForToken
    {
        public string Status { get; set; }
        public string Token { get; set; }
    }
    public class IsTokenValid
    {
        public string token { get; set; }
    }
    public class UserInfoByUserId
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Class { get; set; }

        public string ClassName { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }

    }
    public class IsUserValid
    {
        public string userId { get; set; }
        public string password { get; set; }
    }
    public class LogOn
    {

        [Required(ErrorMessage = "请传入Type")]
        public string PwType { get; set; }
        [Required(ErrorMessage = "请输入用户名")]
        //[RegularExpression(@"/^[-_A-Za-z0-9]+@([_A-Za-z0-9]+\.)+[A-Za-z0-9]{2,3}$/" @"/^1[3|4|5|7|8][0-9]\d{4,8}$/)]
        public string username { get; set; }
        [Required(ErrorMessage = "请输入密码")]
        public string password { get; set; }
        [Required(ErrorMessage = "角色信息必填")]
        public string role { get; set; }
    }

    public class Register
    {
        [Required(ErrorMessage = "请传入Type")]
        public string PwType { get; set; }
        [Required(ErrorMessage = "请输入用户Id")]
        public string userId { get; set; }
        [Required(ErrorMessage = "请输入用户名")]
        //[RegularExpression(@"/^[-_A-Za-z0-9]+@([_A-Za-z0-9]+\.)+[A-Za-z0-9]{2,3}$/" @"/^1[3|4|5|7|8][0-9]\d{4,8}$/)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "请输入密码")]
        public string Password { get; set; }
        [Required(ErrorMessage = "角色信息必填")]
        public string role { get; set; }

        [Required(ErrorMessage = "revUserId")]
        public string revUserId { get; set; }
        [Required(ErrorMessage = "TerminalName")]
        public string TerminalName { get; set; }
        [Required(ErrorMessage = "TerminalIP")]
        public string TerminalIP { get; set; }
        [Required(ErrorMessage = "DeviceType")]
        public int DeviceType { get; set; }
    }

    public class Activation
    {
        [Required(ErrorMessage = "请传入用户Id")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "请输入邀请码")]
        public string InviteCode { get; set; }
        [Required(ErrorMessage = "请输入角色")]
        public string role { get; set; }
    }
    public class ChangePassword
    {
        [Required(ErrorMessage = "请传入OldPassword")]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "请传入NewPassword")]
        public string NewPassword { get; set; }
        [Required(ErrorMessage = "请输入用户Id")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "revUserId")]
        public string revUserId { get; set; }
        [Required(ErrorMessage = "TerminalName")]
        public string TerminalName { get; set; }
        [Required(ErrorMessage = "TerminalIP")]
        public string TerminalIP { get; set; }
        [Required(ErrorMessage = "DeviceType")]
        public int DeviceType { get; set; }

    }

    public class GetPatientsList
    {
        [Required(ErrorMessage = "请传入DoctorId")]
        public string DoctorId { get; set; }
        [Required(ErrorMessage = "请传入ModuleType")]
        public string ModuleType { get; set; }
        [Required(ErrorMessage = "请传入Plan")]
        public int Plan { get; set; }
        [Required(ErrorMessage = "请输入Compliance")]
        public int Compliance { get; set; }
        [Required(ErrorMessage = "请输入Goal")]
        public int Goal { get; set; }


    }
    public class Verification
    {
        [Required(ErrorMessage = "请传入用户Id")]
        public string userId { get; set; }
        [Required(ErrorMessage = "请传入PwType")]
        public string PwType { get; set; }


    }

    public class SetDoctorInfoDetail
    {
        [Required(ErrorMessage = "请传入Doctor")]
        public string Doctor { get; set; }
        [Required(ErrorMessage = "请传入CategoryCode")]
        public string CategoryCode { get; set; }
        [Required(ErrorMessage = "请传入ItemCode")]
        public string ItemCode { get; set; }
        [Required(ErrorMessage = "请输入ItemSeq")]
        public int ItemSeq { get; set; }
        [Required(ErrorMessage = "请输入Value")]
        public string Value { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessage = "请输入SortNo")]
        public int SortNo { get; set; }
        [Required(ErrorMessage = "piUserId")]
        public string piUserId { get; set; }
        [Required(ErrorMessage = "piTerminalName")]
        public string piTerminalName { get; set; }
        [Required(ErrorMessage = "piTerminalIP")]
        public string piTerminalIP { get; set; }
        [Required(ErrorMessage = "piDeviceType")]
        public int piDeviceType { get; set; }


    }
    public class SetPsDoctor
    {
        [Required(ErrorMessage = "请传入UserId")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "请传入UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "请传入Birthday")]
        public int Birthday { get; set; }
        [Required(ErrorMessage = "请输入Gender")]
        public int Gender { get; set; }
        public string IDNo { get; set; }
        [Required(ErrorMessage = "请输入InvalidFlag")]
        public int InvalidFlag { get; set; }
        [Required(ErrorMessage = "piUserId")]
        public string piUserId { get; set; }
        [Required(ErrorMessage = "piTerminalName")]
        public string piTerminalName { get; set; }
 
        public string piTerminalIP { get; set; }
        [Required(ErrorMessage = "piDeviceType")]
        public int piDeviceType { get; set; }


    }
    public class SetPatBasicInfo
    {
        [Required(ErrorMessage = "请传入UserId")]
        public string UserId { get; set; }
        [Required(ErrorMessage = "请传入UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "请传入Birthday")]
        public int Birthday { get; set; }
        [Required(ErrorMessage = "请输入Gender")]
        public int Gender { get; set; }
        //[Required(ErrorMessage = "请输入BloodType")]
        public int BloodType { get; set; }
        //[Required(ErrorMessage = "请输入IDNo")]
        public string IDNo { get; set; }
    
        public string DoctorId { get; set; }
        //[Required(ErrorMessage = "请输入InsuranceType")]
        public string InsuranceType { get; set; }

        //[Required(ErrorMessage = "请输入InvalidFlag")]
        public int InvalidFlag { get; set; }
        [Required(ErrorMessage = "piUserId")]
        public string piUserId { get; set; }
        [Required(ErrorMessage = "piTerminalName")]
        public string piTerminalName { get; set; }
        //[Required(ErrorMessage = "piTerminalIP")]
        public string piTerminalIP { get; set; }
        [Required(ErrorMessage = "piDeviceType")]
        public int piDeviceType { get; set; }


    }
    public class BasicInfo
    {
        public string UserName { get; set; }
        public string Birthday { get; set; }
        public string IDNo { get; set; }
        public string Gender { get; set; }

    }
    public class PatientBasicInfo
    {
        public string UserName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string BloodType { get; set; }
        public string IDNo { get; set; }
        public string DoctorId { get; set; }
        public string InsuranceType { get; set; }
        public string Birthday { get; set; }
        public string GenderText { get; set; }
        public string BloodTypeText { get; set; }
        public string InsuranceTypeText { get; set; }


    }
    public class UserBasicInfo
    {
        public string UserName { get; set; }
        public string Birthday { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string BloodType { get; set; }
        public string IDNo { get; set; }
        public string DoctorId { get; set; }
        public string InsuranceType { get; set; }
        public string InvalidFlag { get; set; }
    }
    public class DoctorInfo
    {
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string Birthday { get; set; }
        public string Gender { get; set; }
        public string IDNo { get; set; }
        public string InvalidFlag { get; set; }


    }
    public class RateTable
    {
        public double PlanRate { get; set; }
        public double ComplianceRate { get; set; }
        public double GoalRate { get; set; }

    }

    public class PatientListTable
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public string photoAddress { get; set; }
        public string PlanNo { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public double Process { get; set; }
        public string RemainingDays { get; set; }
        public List<string> VitalSign { get; set; }
        public double ComplianceRate { get; set; }
        public string TotalDays { get; set; }
        public string Status { get; set; }
        public string VitalValue { get; set; }
        public string VitalUnit { get; set; }
        public string TargetOrigin { get; set; }
        public string TargetValue { get; set; }
        public string SMSCount { get; set; }

        public string Module { get; set; }

    }

    public class PatBasicInfo
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Age { get; set; }
        public string Gender { get; set; }
        public string BloodType { get; set; }
        public string InsuranceType { get; set; }
        public string Birthday { get; set; }
        public string GenderText { get; set; }
        public string BloodTypeText { get; set; }
        public string InsuranceTypeText { get; set; }
        public string Module { get; set; }
        public string DoctorId { get; set; }
        public string IDNo { get; set; }
    }

    public class PatientDetailInfo
    {
        public string UserId { get; set; }
        public string PhoneNumber { get; set; }
        public string HomeAddress { get; set; }
        public string Occupation { get; set; }
        public string Nationality { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyContactPhoneNumber { get; set; }
        public string PhotoAddress { get; set; }
        public string Module { get; set; }
        public string IDNo { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
    }

    public class Calendar
    {
        public string DateTime { get; set; }
        public string Period { get; set; }
        public int SortNo { get; set; }
        public string Description { get; set; }
        public int Status { get; set; }
        public string Redundancy { get; set; }


    }
    public class ActiveUser
    {
        public string UserName { get; set; }
        public string UserId { get; set; }

    }

    public class CategoryByDoctorId
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public string Value { get; set; }
    }

    public class HealthCoachList
    {
        public string healthCoachID { get; set; }
        public string imageURL { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string sex { get; set; }
        public string module { get; set; }
        public string score { get; set; }
    }

    public class HealthCoachInfo
    {
        public string imageURL { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string sex { get; set; }
        public string module { get; set; }
        public string generalComment { get; set; }
        public string generalscore { get; set; }

        public string commentNum { get; set; }
        public string activityDegree { get; set; }
        public string Description { get; set; }

        public string UnitName { get; set; }
        public string Dept { get; set; }
        public string JobTitle { get; set; }

        public string Level { get; set; }

        public int PatientNum { get; set; }

        public int OnPlanPatientNum { get; set; }
        public int DoneCalendarNum { get; set; }
        public int AssessmentNum { get; set; }
        public int MSGNum { get; set; }
        public int AppointmentNum { get; set; }
        public int Activedays { get; set; }
        public string UnitCode { get; set; }
        public string DeptCode { get; set; }


    }

    public class DetailsByDoctorId
    {
        public string CategoryCode { get; set; }
        public string PatientId { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Score { get; set; }
        public string CommentTime { get; set; }
    }

    public class CommentList
    {
        public string CategoryCode { get; set; }
        public string PatientId { get; set; }
        public string imageURL { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public string Score { get; set; }
        public string CommentTime { get; set; }
    }

    public class PatientDetailInfo1 //注意与上面的PatientDetailInfo区分，差个1
    {
        public string PhoneNumber { get; set; }
        public string HomeAddress { get; set; }
        public string Occupation { get; set; }
        public string Nationality { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyContactPhoneNumber { get; set; }
        public string PhotoAddress { get; set; }
        public string IDNo { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
    }

    public class HealthCoachListByPatient
    {
        public string HealthCoachID { get; set; }
        public string imageURL { get; set; }
        public string Name { get; set; }
        public string CategoryCode { get; set; }
        public string module { get; set; }
        public string MessageNo { get; set; }
        public string Content { get; set; }
        public string SendDateTime { get; set; }
        public string SendByName { get; set; }
        public string Flag { get; set; }
        public string Count { get; set; }


    }

    public class DoctorsByPatientId
    {
        public string DoctorId { get; set; }
        public string DoctorName { get; set; }
        public string ItemSeq { get; set; }
        public string ImageURL { get; set; }
        public string Module { get; set; }
        public string MessageNo { get; set; }
        public string Content { get; set; }
        public string SendDateTime { get; set; }
        public string SendByName { get; set; }
        public string Flag { get; set; }
        public string Count { get; set; }

    }


    public class AppoitmentPatient
    {
        public string PatientID { get; set; }
        public string imageURL { get; set; }
        public string name { get; set; }
        public string age { get; set; }
        public string sex { get; set; }
        public string module { get; set; }
        public string AppointmentStatus { get; set; }
        public string Description { get; set; }
        public string ApplicationTime { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentAdd { get; set; }
    }

    public class PatientsByStatus
    {
        public string PatientId { get; set; }
        public string Module { get; set; }
        public string Status { get; set; }
        public string Description { get; set; }
        public string ApplicationTime { get; set; }
        public string AppointmentTime { get; set; }
        public string AppointmentAdd { get; set; }
    }

    public class ReserveHealthCoach
    {
        [Required(ErrorMessage = "请传入DoctorId")]
        public string DoctorId { get; set; }
        [Required(ErrorMessage = "请传入PatientId")]
        public string PatientId { get; set; }
        [Required(ErrorMessage = "请传入Module")]
        public string Module { get; set; }
        [Required(ErrorMessage = "请输入Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "请输入Status")]
        public int Status { get; set; }
        [Required(ErrorMessage = "请输入ApplicationTime")]
        public DateTime ApplicationTime { get; set; }
        [Required(ErrorMessage = "请输入AppointmentTime")]
        public DateTime AppointmentTime { get; set; }
        [Required(ErrorMessage = "请输入AppointmentAdd")]
        public string AppointmentAdd { get; set; }
        public string Redundancy { get; set; }
        [Required(ErrorMessage = "revUserId")]
        public string revUserId { get; set; }
        [Required(ErrorMessage = "TerminalName")]
        public string TerminalName { get; set; }
        [Required(ErrorMessage = "TerminalIP")]
        public string TerminalIP { get; set; }
        [Required(ErrorMessage = "DeviceType")]
        public int DeviceType { get; set; }
    }

    public class UpdateReservation
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }
        public int Status { get; set; }
        public string revUserId { get; set; }
        public string TerminalName { get; set; }
        public string TerminalIP { get; set; }
        public int DeviceType { get; set; }
    }

    public class GetPatientDetailInfo
    {
        public string PhoneNumber { get; set; }
        public string HomeAddress { get; set; }
        public string Occupation { get; set; }
        public string Nationality { get; set; }
        public string EmergencyContact { get; set; }
        public string EmergencyContactPhoneNumber { get; set; }
        public string PhotoAddress { get; set; }
        public string IDNo { get; set; }
    }

    public class GetDoctorInfoDetail
    {
        public string IDNo { get; set; }
        public string PhotoAddress { get; set; }
        public string UnitName { get; set; }

        public string UnitCode { get; set; }
        public string JobTitle { get; set; }
        public string Level { get; set; }
        public string Dept { get; set; }

        public string DeptCode { get; set; }
        public string ActivatePhotoAddr { get; set; }
        public string DeptName { get; set; }

        public string GeneralScore { get; set; }
        public string ActivityDegree { get; set; }

        public string GeneralComment { get; set; }
        public string commentNum { get; set; }
        public string Description { get; set; }

        public int AssessmentNum { get; set; }
        public int MSGNum { get; set; }
        public int AppointmentNum { get; set; }
        public int Activedays { get; set; }
    }

    public class SetComment
    {
        public string DoctorId { get; set; }
        public string CategoryCode { get; set; }
        public string Value { get; set; }
        public string Description { get; set; }
        public string SortNo { get; set; }
        public string piUserId { get; set; }
        public string piTerminalName { get; set; }
        public string piTerminalIP { get; set; }

        public int piDeviceType { get; set; }

    }

    public class PatientNum
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }

    }

    public class Consultation
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }

        public int SortNo { get; set; }
        public DateTime ApplicationTime { get; set; }
        public string HealthCoachId { get; set; }
        public string Module { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ConsultTime { get; set; }
        public string Solution { get; set; }
        public int Emergency { get; set; }

        public int Status { get; set; }
        public string Redundancy { get; set; }

        


        public string revUserId { get; set; }
        public string TerminalName { get; set; }
        public string TerminalIP { get; set; }

        public int DeviceType { get; set; }


    }

    public class ConsultationStatus
    {
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public int PatientGender { get; set; }
        public int PatientAge { get; set; }
        public string Module { get; set; }
        public DateTime ApplicationTime { get; set; }
        public string HealthCoachId { get; set; }
        public string HealthCoachName { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ConsultTime { get; set; }
        public string Solution { get; set; }
        public int Emergency { get; set; }
        public int Status { get; set; }

        public int SortNo { get; set; }
    }

    public class ConsultationChangeStatus
    {
        public string DoctorId { get; set; }
        public string PatientId { get; set; }

        public int SortNo { get; set; }
        public int Status { get; set; }
        public string revUserId { get; set; }
        public string TerminalName { get; set; }
        public string TerminalIP { get; set; }
        public int DeviceType { get; set; }
    }

    public class ConsultationDP
    {
        public int SortNo { get; set; }
        public DateTime ApplicationTime { get; set; }
        public string HealthCoachId { get; set; }
        public string HealthCoachName { get; set; }
        public string Module { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ConsultTime { get; set; }
        public string Solution { get; set; }
        public int Emergency { get; set; }
        public int Status { get; set; }
    }

    public class ConsultationHid
    {
        public string HealthCoachId { get; set; }
        public string HealthCoachName { get; set; }
        public string PatientId { get; set; }
        public string PatientName { get; set; }
        public int Status { get; set; }
        public string DoctorId { get; set; }
        public int SortNo { get; set; }
        public DateTime ApplicationTime { get; set; }
        public string Module { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ConsultTime { get; set; }
        public string Solution { get; set; }
        public int Emergency { get; set; }

    }
}
