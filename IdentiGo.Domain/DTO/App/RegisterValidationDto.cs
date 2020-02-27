using IdentiGo.Domain.Enums;
using System;
using System.Collections.Generic;

namespace IdentiGo.Domain.DTO.App
{
    public class Price
    {
        public double PricePerMessage { get; set; }

        public string Currency { get; set; }
    }

    public class Result
    {
        public string From { get; set; }

        public string To { get; set; }

        public string Text { get; set; }

        public string CleanText { get; set; }

        public object Keyword { get; set; }

        public DateTime ReceivedAt { get; set; }

        public int SmsCount { get; set; }

        public string MessageId { get; set; }

        public object PairedMessageId { get; set; }

        public Price Price { get; set; }

        public object CallbackData { get; set; }
    }

    public class RegisterValidationDto
    {
        public string Result { get; set; }

        public string Message { get; set; }

        public string PhoneAnswer { get; set; }

        public string CodeValidation { get; set; }

        public State State { get; set; }

        public string CodeIVR { get; set; }

        public List<Result> Results { get; set; }

        public int MessageCount { get; set; }

        public int PendingMessageCount { get; set; }

    }

    public class ResponseValidationDto
    {
        public int Result { get; set; }

        public string Message { get; set; }

        public string PhoneAnswer { get; set; }

    }    

    public class RegisterValidationInDto
    {
        public List<Result> Results { get; set; }

        public int MessageCount { get; set; }

        public int PendingMessageCount { get; set; }

        public string Message { get; set; }

        public string PhoneAnswer { get; set; }

        public string CodeValidation { get; set; }

        public State State { get; set; }

        public string CodeIVR { get; set; }
    }
}
