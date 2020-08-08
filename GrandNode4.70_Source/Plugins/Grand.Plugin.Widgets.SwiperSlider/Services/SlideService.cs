using Grand.Core;
using Grand.Core.Data;
using Grand.Core.Infrastructure;
using Grand.Plugin.Widgets.SwiperSlider.Domain;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace Grand.Plugin.Widgets.SwiperSlider.Services
{
    public class SlideService : ISlideService
    {
        private readonly IRepository<Slide> _slideRepository;
        public SlideService(IRepository<Slide> slideRepository)
        {
            _slideRepository = slideRepository;
        }

        /// <summary>
        /// Gets all bar Slides in the app
        /// </summary>
        /// <returns></returns>
        public List<Slide> GetAllSlides()
        {
            return _slideRepository.Table.ToList();
        }

        /// <summary>
        /// returns all roles allowed attributes
        /// </summary>
        /// <param name="sldId">id of the queried role</param>
        /// <returns>list of attributes' ids'</returns>

        public Slide GetSlideById(string sldId)
        {
            return _slideRepository.Table.SingleOrDefault(r => r.Id == sldId);
        }

        /// <summary>
        /// Adds new bar slide
        /// </summary>
        /// <param name="sld"></param>
        /// <returns>1 if the Insert was successful</returns>
        public int Add(Slide sld)
        {
            _slideRepository.Insert(sld);
            return 1;
        }


        /// <summary>
        /// Edits a bar slide
        /// </summary>
        /// <param name="sld">The bar buton to save</param>
        /// <returns></returns>
        public int Update(Slide sld)
        {
            _slideRepository.Update(sld);
            return 1;
        }

        public int Delete(Slide sld)
        {
            _slideRepository.Delete(sld);
            return 1;
        }

    }
}
