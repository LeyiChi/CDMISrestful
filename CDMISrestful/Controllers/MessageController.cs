using CDMISrestful.DataModels;
using CDMISrestful.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CDMISrestful.CommonLibrary;
using System.Web.Http.OData;

namespace CDMISrestful.Controllers
{
     [WebApiTracker]
    [RESTAuthorizeAttribute]
    public class MessageController : ApiController
    {
        static readonly IMessageRepository repository = new MessageRepository();
        DataConnection pclsCache = new DataConnection();
        /// <summary>
        /// GetSMSDialogue 获取消息对话 GL 2015-10-10
        /// </summary>
        /// <param name="Reciever"></param>
        /// <param name="SendBy"></param>
        /// <returns></returns>
        
        [Route("Api/v1/MessageInfo/messages")]
        [EnableQuery]
        public List<Message> GetSMSDialogue(string Reciever, string SendBy)
        {
            return repository.GetSMSDialogue(pclsCache, Reciever, SendBy);
        }

        /// <summary>
        /// PutSMSRead 将多条消息设为已读 GL 2015-10-10（修改2015-10-26）
        /// </summary>
        /// <param name="SendBy"></param>
        /// <param name="Reciever"></param>
        /// <param name="Content"></param>
        /// <param name="piUserId"></param>
        /// <param name="piTerminalName"></param>
        /// <param name="piTerminalIP"></param>
        /// <param name="piDeviceType"></param>
        /// <returns></returns>
        [Route("Api/v1/MessageInfo/message")]
        [ModelValidationFilter]
        public HttpResponseMessage PutSMSRead(Message item)
        {
            int ret = repository.SetSMSRead(pclsCache, item.Receiver, item.SendBy, item.piUserId, item.piTerminalName, new CommonFunction().getRemoteIPAddress(), item.piDeviceType);                       
            return new ExceptionHandler().SetData(Request,ret);
        }
        /// <summary>
        /// PostSMS 将消息写入数据库并获取发送时间与显示时间 GL 2015-10-26
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [Route("Api/v1/MessageInfo/message")]
        public Message PostSMS(Message item)
        {
            return repository.SetSMS(pclsCache, item.SendBy, item.Receiver, item.Content, item.piUserId, item.piTerminalName, new CommonFunction().getRemoteIPAddress(), item.piDeviceType);
        }

        /// <summary>
        /// GetLatestSMS 获取最新一条消息 GL 2015-10-10
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <param name="PatientId"></param>
        /// <returns></returns>
        [Route("Api/v1/MessageInfo/message")]
        public Message GetLatestSMS(string DoctorId, string PatientId)
        {
            return repository.GetLatestSMS(pclsCache, DoctorId, PatientId);
        }

        /// <summary>
        /// GetSMSCount 获取未读消息数 GL 2015-10-10
        /// </summary>
        /// <param name="Reciever"></param>
        /// <param name="SendBy"></param>
        /// <returns></returns>
        [Route("Api/v1/MessageInfo/messageNum")]
        public HttpResponseMessage GetSMSCount(string Reciever, string SendBy)
        {
            string ret = "0";
            if (SendBy == "NULL")
            {
                ret = repository.GetSMSCountForAll(pclsCache, Reciever).ToString();
            }
            else
            {
                ret = repository.GetSMSCountForOne(pclsCache, Reciever, SendBy).ToString();
            }
            return new ExceptionHandler().Common(Request, ret);
        }

        /// <summary>
        /// GetSMSList 获取消息联系人列表 GL 2015-10-10
        /// </summary>
        /// <param name="DoctorId"></param>
        /// <param name="CategoryCode"></param>
        /// <returns></returns>
        [Route("Api/v1/MessageInfo/messageContact")]
        [EnableQuery]
        public List<Message> GetSMSList(string DoctorId, string CategoryCode)
        {
            return repository.GetSMSList(pclsCache, DoctorId, CategoryCode);
        }

        /// <summary>
        /// 提醒表PsNotification插入数据 syf 20151215
        /// </summary>
        /// <param name="Item"></param>
        /// <returns></returns>
        [Route("Api/v1/MessageInfo/PostNotification")]
        [ModelValidationFilter]
        public HttpResponseMessage PostNotification(PsNotification Item)
        {
            int ret = repository.PsNotificationSetData(pclsCache, Item.AccepterID, Item.NotificationType, Item.Title, Item.Description, Item.SendTime, Item.SenderID, Item.Status, Item.Redundance, Item.piUserId, Item.piTerminalName, Item.piTerminalIP, Item.piDeviceType);
            return new ExceptionHandler().SetData(Request, ret);
        }

        /// <summary>
        /// 改变消息状态 SYF 20151215
        /// </summary>
        /// <param name="AccepterID"></param>
        /// <param name="NotificationType"></param>
        /// <param name="SortNo"></param>
        /// <param name="Status"></param>
        /// <param name="revUserId"></param>
        /// <param name="TerminalName"></param>
        /// <param name="TerminalIP"></param>
        /// <param name="DeviceType"></param>
        /// <returns></returns>
        [Route("Api/v1/MessageInfo/ChangeStatus")]
        [ModelValidationFilter]
        public HttpResponseMessage ChangeStatus(NotificationStatus item)
        {
            int ret = repository.PsNotificationChangeStatus(pclsCache, item.AccepterID, item.NotificationType, item.SortNo, item.Status, item.piUserId, item.piTerminalName, item.piTerminalIP, item.piDeviceType);
            return new ExceptionHandler().ChangeStatus(Request, ret);
        }

        /// <summary>
        /// 根据Status取数据——SYF 20151215
        /// </summary>
        /// <param name="AccepterID"></param>
        /// <param name="NotificationType"></param>
        /// <param name="Status"></param>
        /// <returns></returns>
        [Route("Api/v1/MessageInfo/GetDataByStatus")]
        [ModelValidationFilter]
        [EnableQuery]
        public List<PsNotification> GetDataByStatus(string AccepterID, string NotificationType, string Status)
        {
            return repository.PsNotificationGetDataByStatus(pclsCache, AccepterID, NotificationType, Status);
        }
        
         /// <summary>
         /// 获取未读消息数量，Status填0为未读，施宇帆 20151222
         /// </summary>
         /// <param name="AccepterID"></param>
         /// <param name="NotificationType"></param>
         /// <param name="Status"></param>
         /// <returns></returns>
        [Route("Api/v1/MessageInfo/UnreadNums")]
        public int GetUnreadNum(string AccepterID, string NotificationType, string Status)
        {
            int ret = repository.PsNotificationGetUnreadNum(pclsCache, AccepterID, NotificationType, Status);
            return ret;
        }


    }
}
