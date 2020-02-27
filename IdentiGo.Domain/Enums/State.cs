using System.ComponentModel.DataAnnotations;

namespace IdentiGo.Domain.Enums
{
    public enum State
    {
        [Display(Name = "Inicializado")]
        Default = 0,
        [Display(Name = "Exitoso")]
        Success = 1,
        [Display(Name = "Invalido")]
        Invalid = 2,
        [Display(Name = "Bloqueado un Día")]
        LockedDay = 3,
        [Display(Name = "Bloqueado")]
        Locked = 4,
        [Display(Name = "inhabilitado")]
        Disabled = 5,
        [Display(Name = "Pendiente Confirmación")]
        Pending = 6,
        [Display(Name = "No contactada")]
        NotContacted = 13,
        [Display(Name = "Exitoso")]
        Pass = 14,
    }

    public enum StateDocument
    {
        [Display(Name = " ")]
        Default = 0,
        [Display(Name = "WriteOff")]
        WriteOff = 4,
        [Display(Name = "Nueva")]
        New = 5,
        [Display(Name = "Collection")]
        Collection = 7,
        [Display(Name = "Pago anticipado")]
        PagoAnticipado = 8,
        [Display(Name = "Activa")]
        Activa = 9,
        [Display(Name = "Inactiva")]
        Inactiva = 10,
        [Display(Name = "Incumplimiento de políticas Avon")]
        ListOFAC = 11,
        [Display(Name = "No Vigente")]
        NoVigente = 12,
        [Display(Name = "Pago contado")]
        PagoContado = 13
    }
}
