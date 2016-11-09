using CDMISrestful.CommonLibrary;
using CDMISrestful.DataMethod;
using CDMISrestful.DataModels;
using InterSystems.Data.CacheClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CDMISrestful.Models
{
    public class MessageRepository : IMessageRepository
    {
       
        /// <summary>
        /// 获取消息对话 GL 2015-10-10
        /// </summary>
        /// <param name="Reciever"></param>
        /// <param name="SendBy"></param>
        /// <returns></returns>
        public List<Message> GetSMSDialogue(DataConnection pclsCache, string Reciever, string SendBy)
        {
            return new MessageMethod().GetSMSDialogue(pclsCache, Reciever, SendBy);
        }

        /// <summary>
        /// 将消息写入数据库并获取发送时间与显示时间 GL 2015-10-10(修改：2015-10-26)
        /// </summary>
        /// <param name="SendBy"></param>
        /// <param name="Reciever"></param>
        /// <param name="Content"></param>
        /// <param name="piUserId"></param>
        /// <param name="piTerminalName"></param>
        /// <param name="piTerminalIP"></param>
        /// <param name="piDeviceType"></param>
        /// <returns></returns>
        public Message SetSMS(DataConnection pclsCache, string SendBy, string Reciever, string Content, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            return new MessageMethod().SetSMS(pclsCache, SendBy, Reciever, Content, piUserId, piTerminalName, piTerminalIP, piDeviceType);         
        }

        /// <summary>
        /// 获取最新一条消息 GL 2015-10-10
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        public Message GetLatestSMS(DataConnection pclsCache, string DoctorId, string PatientId)
        {
            return new MessageMethod().GetLatestSMS(pclsCache, DoctorId, PatientId);             
        }

        /// <summary>
        /// 将多条消息设为已读 GL 2015-10-10
        /// </summary>
        /// <param name="Reciever"></param>
        /// <param name="SendBy"></param>
        /// <param name="piUserId"></param>
        /// <param name="piTerminalName"></param>
        /// <param name="piTerminalIP"></param>
        /// <param name="piDeviceType"></param>
        /// <returns></returns>
        public int SetSMSRead(DataConnection pclsCache, string Reciever, string SendBy, string piUserId, string piTerminalName, string piTerminalIP, int piDeviceType)
        {
            return new MessageMethod().SetSMSRead(pclsCache, Reciever, SendBy, piUserId, piTerminalName, piTerminalIP, piDeviceType);       
        }

        /// <summary>
        /// 获取一对一未读消息数 GL 2015-10-10
        /// </summary>
        /// <param name="Reciever"></param>
        /// <param name="SendBy"></param>
        /// <returns></returns>
        public int GetSMSCountForOne(DataConnection pclsCache, string Reciever, string SendBy)
        {
            return new MessageMethod().GetSMSCountForOne(pclsCache, Reciever, SendBy);         
        }

        /// <summary>
        /// 获取消息联系人列表 GL 2015-10-10
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <param name="CategoryCode"></param>
        /// <returns></returns>
        public List<Message> GetSMSList(DataConnection pclsCache, string DoctorId, string CategoryCode)
        {
            return new MessageMethod().GetSMSList(pclsCache, DoctorId, CategoryCode);
        }

        /// <summary>
        /// 获取某医生未读消息总数 GL 2015-10-10
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <returns></returns>
        public int GetSMSCountForAll(DataConnection pclsCache, string DoctorId)
        {
            return new MessageMethod().GetSMSCountForAll(pclsCache, DoctorId);        
        }


        public int PsNotificationSetData(DataConnection pclsCache, string AccepterID, string NotificationType, string Title, string Description, string SendTime, string SenderID, string Status, string Redundancy, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            return new MessageMethod().PsNotificationSetData(pclsCache, AccepterID, NotificationType, Title, Description, SendTime, SenderID, Status, Redundancy, revUserId, TerminalName, TerminalIP, DeviceType);
        }

        public int PsNotificationChangeStatus(DataConnection pclsCache, string AccepterID, string NotificationType, int SortNo, string Status, string revUserId, string TerminalName, string TerminalIP, int DeviceType)
        {
            return new MessageMethod().PsNotificationChangeStatus(pclsCache, AccepterID, NotificationType, SortNo, Status, revUserId, TerminalName, TerminalIP, DeviceType);
        }


        public List<PsNotification> PsNotificationGetDataByStatus(DataConnection pclsCache, string AccepterID, string NotificationType, string Status)
        {
            return new MessageMethod().PsNotificationGetDataByStatus(pclsCache, AccepterID, NotificationType, Status);
        }

        public int PsNotificationGetUnreadNum(DataConnection pclsCache, string AccepterID, string NotificationType, string Status)
        {
            return new MessageMethod().PsNotificationGetUnreadNum(pclsCache, AccepterID, NotificationType, Status);
        }
    }
}