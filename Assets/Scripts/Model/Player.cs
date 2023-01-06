using System.Collections;
using System.Collections.Generic;

namespace GameModel
{
    public class Player
    {
        private string username;
        private int ID;
        private string _name;
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private int _resourceValue = 1000;
        public int resourceValue
        {
            get
            {
                return _resourceValue;
            }
            set
            {
                _resourceValue = value;
            }
        }

        private int _currentResourcePower = 50;
        public int currentResourcePower {
            set{
                _currentResourcePower = value;
            }
            get{
                return _currentResourcePower;
            }
        }

        private int _mainBaseHP = 100;
        public int mainBaseHp
        {
            get
            {
                return _mainBaseHP;
            }
            set
            {
                _mainBaseHP = value;
            }
        }

        private int _unitCount = 0;
        public int unitCount
        {
            get
            {
                return _unitCount;
            }
            set
            {
                _unitCount = value;
            }
        }

        public void AddResources() {
            this.resourceValue += this._currentResourcePower;
        }

        public void IncreaseResourcePower(int resource){
            currentResourcePower += resource;
        }
        public void IncreaseResourceValue(int resource){
            this.resourceValue += resource;
        }

        public void DecreaseResourcePower(int resource){
            currentResourcePower -= resource;
        }
    }
}
