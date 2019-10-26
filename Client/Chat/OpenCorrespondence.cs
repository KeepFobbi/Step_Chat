using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat
{
    public class OpenCorrespondence
    {
        public string statusType; // chat or group
        public int idCorr; // selected id

        public OpenCorrespondence(string statusType, int idCorr)
        {
            this.statusType = statusType;
            this.idCorr = idCorr;
        }
    }
}
