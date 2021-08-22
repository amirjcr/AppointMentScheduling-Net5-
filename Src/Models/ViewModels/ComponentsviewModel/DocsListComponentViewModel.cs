using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Src.Services.Interfaces;
using System.Collections.Generic;
using Src.Models.ViewModels.Doctor;

namespace Src.Models.ViewModels.ComponentsViewModel
{
    [ViewComponent(Name = "DocsListViewComponent")]
    public class DocsListComponentViewModel : ViewComponent
    {

        private readonly IAppointmentService _service;
        public DocsListComponentViewModel(IAppointmentService service)
        {
            _service = service;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var sourcelist = await _service.GetDoctorList();
            var lst = new List<SelectListItem>();

            foreach (var item in sourcelist)
                lst.Add(new SelectListItem { Value = item.Id, Text = item.Name });

            return View("_docList", lst);
        }
    }
}