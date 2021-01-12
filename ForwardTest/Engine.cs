using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForward
{
    interface Engine
    {
        double Cur_T
        {
            get;
            set;
        }
        double Crit_T
        {
            get;
            set;
        }
        public double Start(double external_T, double timeLimit, double deltaTime = 0.001);
    }
    class InternalCombustionEngineConfig
    {
        public double I { get; set; }
        public double[] M { get; set; }
        public double[] V { get; set; }
        public double Crit_T { get; set; }
        public double H_m { get; set; }
        public double H_v { get; set; }
        public double C { get; set; }
    }
    class InternalCombustionEngine : Engine

    {
        public double I;
        public double[] M;
        public double[] V;
        public double cur_V = 0.0;
        public double Cur_T { get => cur_T; set => cur_T = value; }
        public double Crit_T { get => crit_T; set => crit_T = value; }

        public double H_m;
        public double H_v;
        public double C;

        private double a;

        private double V_h;
        private double V_c;
        private double cur_T;
        private double crit_T;

        public InternalCombustionEngine(double I, double[] M, double[] V, double Crit_T, double H_m, double H_v, double C)
        {
            this.I = I;
            this.M = M;
            this.V = V;
            this.Crit_T = Crit_T;
            this.H_m = H_m;
            this.H_v = H_v;
            this.C = C;
            
        }
        public InternalCombustionEngine(InternalCombustionEngineConfig config)
        {
            this.I = config.I;
            this.M = config.M;
            this.V = config.V;
            this.Crit_T = config.Crit_T;
            this.H_m = config.H_m;
            this.H_v = config.H_v;
            this.C = config.C;

        }
        private double GetCurrentM()
        {
            double k;
            double b;
            for(int i = 0; i < M.Length; i++)
            {
                if (V[i] >= cur_V)
                {
                    if (i != M.Length - 1)
                    {
                        k = (M[i] - M[i + 1]) / (V[i] - V[i + 1]);
                        b = k * V[i] - M[i];
                        return k * cur_V - b;
                    }
                    else
                        return M[i];
                }
            }
            return 0.0;
        }
        private void Tick(double external_T, double deltaTime)
        {
            double m = GetCurrentM();
            a = m / I;
            cur_V += a * deltaTime;
            V_h = m * H_m + Math.Pow(cur_V, 2) * H_v;
            V_c = C * (external_T - cur_T);
            cur_T += V_h * deltaTime + V_c * deltaTime; 
        }
        public double Start(double external_T, double timeLimit, double deltaTime=0.001)
        {
            double time = 0.0;
            cur_T = external_T;
            while (cur_T < crit_T)
            {
                Tick(external_T, deltaTime);
                time += deltaTime;
                if (time > timeLimit)
                    throw new Exception("Превышен лимит времени");
            }
            return time;
        }
    }
}
