using System;
using System.Collections.Generic;
using System.Linq;

namespace ijw.CUI
{
    public class CuiWindow
    {
        public string Title { get; set; }
        public Alignment TitleAlignment { get; set; }
        public int PosX { get; set; }
        public int PosY { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Fullscreen { get; set; }
        public List<CuiWindow> SubWindows { get; }
        public CuiWindow FocusedWindow { get; set; } 

        public void Render() {
            this.renderSelf();

            foreach (var item in SubWindows) {
                item.Render();
            }
        }

        private void renderSelf() {

            this.drawRectangle(PosX, PosY, Width, Height);
        }

        private void drawRectangle(int posX, int posY, int width, int height) {
            throw new NotImplementedException();
        }
    }
}
