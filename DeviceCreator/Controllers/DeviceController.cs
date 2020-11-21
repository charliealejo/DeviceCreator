using System;
using System.Web.Mvc;
using DeviceCreator.App_LocalResources;
using DeviceCreator.Models.Factory;
using DeviceCreator.Models.Web;
using DeviceManager;

namespace DeviceCreator.Controllers
{
    /// <summary>
    /// The controller for operations with the devices.
    /// </summary>
    public class DeviceController : Controller
    {
        /// <summary>
        /// The instance of the device factory.
        /// </summary>
        IDeviceFactory deviceFactory;

        /// <summary>
        /// The instance of the database publisher.
        /// </summary>
        IDevicePublisher dbAccess;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:DeviceCreator.Controllers.DeviceController"/> class.
        /// </summary>
        /// <param name="dbAccess">The instance of the interface for accessing the DB.</param>
        /// <param name="deviceFactory">The instance of the device factory.</param>
        public DeviceController(IDevicePublisher dbAccess, IDeviceFactory deviceFactory)
        {
            this.dbAccess = dbAccess;
            this.deviceFactory = deviceFactory;
        }

        /// <summary>
        /// Creates a view with a list of devices.
        /// </summary>
        /// <returns>The view.</returns>
        public ActionResult Index()
        {
            return View(dbAccess.GetDevices());
        }

        /// <summary>
        /// Creates a view with a form for creating new devices.
        /// </summary>
        /// <returns>The view.</returns>
        public ActionResult Create()
        {
            return View();
        } 

        /// <summary>
        /// Validates the form with the new device data and saves it if
        /// it contains valid data. Otherwise it returns to the form view.
        /// </summary>
        /// <returns>A view.</returns>
        /// <param name="form">The new device data.</param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DeviceForm form)
        {
            try {
                if (form.Type == DeviceType.GATEWAY &&
                    string.IsNullOrWhiteSpace(form.IP))
                {
                    ModelState.AddModelError("IP", Resources.IPRequired);
                }
                else if (ModelState.IsValid)
                {
                    if (dbAccess.SaveDevice(deviceFactory.CreateDevice(form)))
                    {
                        return RedirectToAction("Index");
                    }
                    ModelState.AddModelError("ID", Resources.IDAndSNPresent);
                    return View(form);
                }
                return View(form);
            }
            catch
            {
                ViewData["ErrorMessage"] = Resources.ErrorWhileSavingDevice;
                return View();
            }
        }
    }
}