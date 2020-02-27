using Component.Transversal.Utilities;
using IdentiGo.Domain.DTO;
using IdentiGo.Domain.Enums;
using IdentiGo.Services.General;
using IdentiGo.Transversal.Utilities;
using IdentiGo.WebManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IdentiGo.Domain.Entity;
using IdentiGo.Domain.Entity.General;
using IdentiGo.Services.Master;
using IdentiGo.WebManagement.Security;

namespace IdentiGo.WebManagement.Areas.General.Controllers
{

    [Authorize(Roles = RoleName.Role1)]
    public class ConfigController : Controller
    {
        public readonly IConfigService ConfigService;

        public ConfigController(IConfigService configService)
        {
            ConfigService = configService;
        }

        //
        // GET: /Config/
        [AuthorizeRoles]
        public ActionResult Index()
        {
            var configList = ConfigService.GetAll();
            return View(configList);
        }
        
        // GET: /Config/Edit/1
        public ActionResult Edit()
        {
            var config = ConfigService.GetAll().FirstOrDefault();
            if (config == null)
            {
                config = new Config();
                ConfigService.AddOrUpdate(config);
            }

            return View(config);
        }

        //
        // POST: /Config/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Config config)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var currentItem = ConfigService.Get(config.Id);

                    currentItem.UserCIFIN = config.UserCIFIN;
                    currentItem.PasswordCIFIN = config.PasswordCIFIN;
                    currentItem.EmailTo = currentItem.EmailTo;
                    currentItem.Subject = currentItem.Subject;

                    ConfigService.Update(config);
                }
                catch (Exception ex) { ModelState.AddModelError("", ex.Message); }
            }
            return View(config);

        }
        
    }
}