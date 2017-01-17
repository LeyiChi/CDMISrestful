using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CDMISrestful.DataModels;
using CDMISrestful.DataViewModels;
using CDMISrestful.CommonLibrary;

namespace CDMISrestful.Models
{
    public interface IUsersRepository
    {
        bool IsTokenValid(string token);
        ForToken LogOn(DataConnection pclsCache, string PwType, string userId, string password, string role);
        //string IsUserValid(string userId, string password);

        List<Doctor> GetDoctorList(DataConnection pclsCache);

        int Register(DataConnection pclsCache, string PwType, string userId, string UserName, string Password, string role, string revUserId, string TerminalName, string TerminalIP, int DeviceType);
        int Activition(DataConnection pclsCache, string UserId, string InviteCode, string role);
        int ChangePassword(DataConnection pclsCache, string OldPassword, string NewPassword, string UserId, string revUserId, string TerminalName, string TerminalIP, int DeviceType);
        //PatientsDataSet GetPatientsList(string DoctorId, string ModuleType, int Plan, int Compliance, int Goal);
        int Verification(DataConnection pclsCache, string userId, string PwType);
        PatBasicInfo GetPatBasicInfo(DataConnection pclsCache, string UserId);
        PatientDetailInfo GetPatientDetailInfo(DataConnection pclsCache, string UserId);
        DocInfoDetail GetDoctorDetailInfo(DataConnection pclsCache, string UserId);

        DoctorInfo GetDoctorInfo(DataConnection pclsCache, string DoctorId);
        int SetDoctorInfoDetail(DataConnection pclsCache, string Doctor, string CategoryCode, string ItemCode, int ItemSeq, string Value, string Description, int SortNo, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType);
        int SetPsDoctor(DataConnection pclsCache, string UserId, string UserName, int Birthday, int Gender, string IDNo, int InvalidFlag, string revUserId, string TerminalName, string TerminalIP, int DeviceType);

        int SetPatBasicInfo(DataConnection pclsCache, string UserId, string UserName, int Birthday, int Gender, int BloodType, string IDNo, string DoctorId, string InsuranceType, int InvalidFlag, string revUserId, string TerminalName, string TerminalIP, int DeviceType);
        int SetPatBasicInfoDetail(DataConnection pclsCache, string Patient, string CategoryCode, string ItemCode, int ItemSeq, string Value, string Description, int SortNo, string revUserId, string TerminalName, string TerminalIP, int DeviceType);


        string GetIDByInputPhone(DataConnection pclsCache, string Type, string Name);
        List<Calendar> GetCalendar(DataConnection pclsCache, string DoctorId);

        List<HealthCoachList> GetHealthCoachList(DataConnection pclsCache);
        HealthCoachInfo GetHealthCoachInfo(DataConnection pclsCache, string HealthCoachID);

        int ReserveHealthCoach(DataConnection pclsCache, string DoctorId, string PatientId, string Module, string Description, int Status, DateTime ApplicationTime, DateTime AppointmentTime, string AppointmentAdd, string Redundancy, string revUserId, string TerminalName, string TerminalIP, int DeviceType);

        int UpdateReservation(DataConnection pclsCache, string DoctorId, string PatientId, int Status, string revUserId, string TerminalName, string TerminalIP, int DeviceType);

        List<CommentList> GetCommentList(DataConnection pclsCache, string DoctorId, string CategoryCode);

        int RemoveHealthCoach(DataConnection pclsCache, string PatientId, string DoctorId, string CategoryCode);

        List<HealthCoachListByPatient> GetHealthCoachListByPatient(DataConnection pclsCache, string PatientId);

        string GetPhoneNoByUserId(DataConnection pclsCache, string UserId);

        List<string> GetAllRoleMatch(DataConnection pclsCache, string UserId);
        List<AppoitmentPatient> GetAppoitmentPatientList(DataConnection pclsCache, string healthCoachID, string Status);
        List<PatientListTable> GetPatientsPlan(DataConnection pclsCache, string DoctorId, string Module, string VitalType, string VitalCode);

        string GetBasicInfoDtlValue(DataConnection pclsCache, string UserId, string CategoryCode, string ItemCode, int ItemSeq);

        int PsCalendarSetData(DataConnection pclsCache, string DoctorId, int DateTime, string Period, int SortNo, string Description, int Status, string Redundancy, string revUserId, string TerminalName, string TerminalIP, int DeviceType);

        int SetComment(DataConnection pclsCache, string DoctorId, string CategoryCode, string Value, string Description, string SortNo, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType);

        List<ModulesByPID> GetHModulesByID(DataConnection pclsCache, string PatientId, string DoctorId);

        string GetValueByType(DataConnection pclsCache, string UserId, string Type);

        int PsConsultationSetData(DataConnection pclsCache, string DoctorId, string PatientId, int SortNo, DateTime ApplicationTime, string HealthCoachId, string Module, string Title, string Description, DateTime ConsultTime, string Solution, int Emergency, int Status, string Redundancy, string revUserId, string TerminalName, string TerminalIP, int DeviceType);

        List<ConsultationStatus> ConsultationGetPatientsByStatus(DataConnection pclsCache, string DoctorId, int Status);

        int ConsultationChangeStatus(DataConnection pclsCache, string DoctorId, string PatientId, int SortNo, int Status, string revUserId, string TerminalName, string TerminalIP, int DeviceType);

        List<ConsultationDP> ConsultationGetDataByDP(DataConnection pclsCache, string DoctorId, string PatientId);
    }
}