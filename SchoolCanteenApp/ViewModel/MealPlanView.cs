using SchoolCanteenApp.ViewModel;
using System;

namespace SchoolCanteenApp.ViewModels
{
    internal class MealPlanView
    {
        private MealPlanViewModel vm;

        public MealPlanView(MealPlanViewModel vm)
        {
            this.vm = vm;
        }

        internal bool ShowDialog()
        {
            throw new NotImplementedException();
        }
    }
}