using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using PropertyChanged;
using Tamarillo.Classes.Helpers.Extensions;

namespace Tamarillo.Classes.Models.Base {

    [AddINotifyPropertyChangedInterface]
    public class Model {

        protected void Run(Expression<Func<bool>> flag, Action action) {

            if (null == flag) {
                throw new NotSupportedException($"Flag not specified! '{nameof(flag)}'");
            }

            if (null == action) {
                throw new NotSupportedException($"Action not specified! '{nameof(action)}'");
            }

            if (flag.GetFlag())
                return;

            flag.SetFlag(true);
            try {
                action.Invoke();
            }
            finally {
                flag.SetFlag(false);
            }

        }
        protected async Task RunAsync(Expression<Func<bool>> flag, Func<Task> action) {

            if (null == flag) {
                throw new NotSupportedException($"Flag not specified! '{nameof(flag)}'");
            }

            if (null == action) {
                throw new NotSupportedException($"Action not specified! '{nameof(action)}'");
            }

            if (!flag.GetFlag()) {
                flag.SetFlag(true);
                try {
                    await action();
                }
                finally {
                    flag.SetFlag(false);
                }
            }

        }

    }

}
