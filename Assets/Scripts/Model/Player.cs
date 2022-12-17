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
    }
}
