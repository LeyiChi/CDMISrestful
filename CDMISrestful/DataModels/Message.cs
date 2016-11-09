using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CDMISrestful.DataModels
{
    public class Message
    {
        /// <summary>
        /// 消息编号
        /// </summary>
        public string MessageNo { get; set; }
        /// <summary>
        /// 消息类型
        /// </summary>
        public int MessageType { get; set; }
        /// <summary>
        /// 发送状态
        /// </summary>
        public int SendStatus { get; set; }
        /// <summary>
        ///阅读状态
        /// </summary>
        public int ReadStatus { get; set; }
        /// <summary>
        /// 发送者Id
        /// </summary>
        [Required]
        public string SendBy { get; set; }
        /// <summary>
        /// 发送者姓名
        /// </summary>
        public string SendByName { get; set; }
        /// <summary>
        /// 发送时间
        /// </summary>
        public string SendDateTime { get; set; }
        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 接收者Id
        /// </summary>
        [Required]
        public string Receiver { get; set; }
        /// <summary>
        /// 接收者姓名
        /// </summary>
        public string ReceiverName { get; set; }
        /// <summary>
        /// 短信标志
        /// </summary>
        public int SMSFlag { get; set; }

        /// <summary>
        /// 区分接收和发送
        /// </summary>
        public string IDFlag { get; set; }

        /// <summary>
        ///标志位
        /// </summary>
        public string Flag { get; set; }

        /// <summary>
        /// 未读消息数
        /// </summary>
        public string Count { get; set; }
        /// <summary>
        /// 显示时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
       /// 以下4个字段为更新信息输入
       /// </summary>
       public string piUserId{ get; set; }       
       public string piTerminalName{ get; set; }
       public string piTerminalIP{ get; set; }
       public int piDeviceType{ get; set; } 
    }


    public class PsNotification
    {
        public string AccepterID { get; set; }
        public string NotificationType { get; set; }
        public string SortNo { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string SendTime { get; set; }
        public string SenderID { get; set; }

        public string SenderName { get; set; }

        public string Status { get; set; }
        public string Redundance { get; set; }
        public string piUserId { get; set; }
        public string piTerminalName { get; set; }
        public string piTerminalIP { get; set; }
        public int piDeviceType { get; set; }
    }


    public class NotificationStatus
    {
        public string AccepterID { get; set; }
        public string NotificationType { get; set; }
        public int SortNo { get; set; }
        public string Status { get; set; }
        public string piUserId { get; set; }
        public string piTerminalName { get; set; }
        public string piTerminalIP { get; set; }
        public int piDeviceType { get; set; }
    }
}