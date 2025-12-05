using System;
using System.ComponentModel.DataAnnotations;

namespace web.Models { 

    public class Valera
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Valera";
        public bool is_alive { get; set; } = true;
        [Range(0, 100)]
        public int HP { get; set; } = 100;
        [Range(0, 100)]
        public int MP { get; set; } = 0;
        [Range(0, 100)]
        public int FT { get; set; } = 0;
        public const int MaxCommonStats = 100, minCommonStats = 0;
        [Range(-10, 10)]
        public int CF { get; set; } = 0;
        [Range(0, int.MaxValue)]
        public int MN { get; set; } = 0;

        public const int maxCF = 10, minCF = -10;
        public Valera() { }
        public Valera(int hP, int mP, int fT, int cF, int mN, string name = "Valera")
        {
            HP = hP; MP = mP; FT = fT; MN = mN; CF = cF; Name = name;
            stat_checker();
        }
        private void stat_checker() {
            if (HP > MaxCommonStats) HP = MaxCommonStats;
            if (HP < minCommonStats) { is_alive = false; }
            if (MP > MaxCommonStats) MP = MaxCommonStats;
            if (MP < minCommonStats) MP = minCommonStats;
            if (FT > MaxCommonStats) FT = MaxCommonStats;
            if (FT < minCommonStats) FT = minCommonStats;
            if (CF > maxCF) CF = maxCF;
            if (CF < minCF) CF = minCF;
        }
        public void go_to_work() {
            if (is_alive)
                if (MP < 50 && FT < 10)
                {
                    CF -= 5;
                    MP -= 30;
                    MN += 100;
                    FT += 70;
                    stat_checker();
                }
        }
        public void go_to_touch_grass() {
            if (is_alive)
            {
                CF += 1;
                MP -= 10;
                FT += 10;
                stat_checker();
            }
        }
        public void go_to_cinema() {
            if (is_alive)
            {
                CF -= 1;
                MP += 30;
                FT += 10;
                HP -= 5;
                MN -= 20;
                stat_checker();
            }
        }
        public void go_to_pub() {
            if (is_alive)
            {
                CF += 1;
                MP += 60;
                FT += 40;
                HP -= 10;
                MN -= 100;
                stat_checker();
            }
            }
        public void go_to_drink_with() {
            if (is_alive)
            {
                CF += 5;
                HP -= 80;
                MP += 90;
                FT += 80;
                MN -= 150;
                stat_checker();
            }
        }
        public void go_sing_in_metro() {
            if (is_alive)
            {
                CF += 1;
                MP += 10;
                MN += MP > 40 && MP < 70 ? 60 : 10;
                FT += 20;
                stat_checker();
            }
        }
        public void go_to_sleep() {
            if (is_alive)
            {
                HP += MP < 30 ? 90 : 0;
                CF -= MP > 70 ? 3 : 0;
                MP -= 50;
                FT -= 70;
                stat_checker();
            }
        }
    }
}
