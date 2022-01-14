using System;
using System.Device.Pwm;
using System.Threading.Tasks;
using Iot.Device.ServoMotor;

namespace CatToyWebApp.Hardware
{
    public class Servo
    {
        public PwmChannel pwmChannel { get; set; }
        public ServoMotor ToyServo { get; set; }
        private int StartingPulseWidth { get; set; }

        public async Task MotorTestAsync()
        {
            ToyServo.Start();

            for (int i = 0; i < 2; i++)
            {
                if (!Startup.IsShuttingDown)
                {
                    Console.WriteLine($"Running test number {i}");
                    ToyServo.WritePulseWidth(1500);
                    await Task.Delay(500);
                    ToyServo.WritePulseWidth(2250);
                    await Task.Delay(500);
                    ToyServo.WritePulseWidth(1500);
                    await Task.Delay(500);
                    ToyServo.WritePulseWidth(1100);
                    await Task.Delay(500);
                    ToyServo.WritePulseWidth(1500);
                    await Task.Delay(500);
                    ToyServo.WritePulseWidth(1100);
                    await Task.Delay(500);
                    ToyServo.WritePulseWidth(2250);
                    await Task.Delay(3000);
                }
            }
            ResetPosition();
        }

        public void ResetPosition()
        {
            ToyServo.WritePulseWidth(2400);
            Task.Delay(2500);
            ToyServo.Stop();
        }

        public Servo()
        {
            pwmChannel = PwmChannel.Create(0, 0, 50);
            ToyServo = new ServoMotor(pwmChannel, 270, 900, 2400);

            StartingPulseWidth = 2400;
        }
    }
}
