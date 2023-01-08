function renderPixels(display, color, background) {
  return display.map(p => p === 'x' ? color : background);
}

function renderAnimation(display, durationInSeconds, animation) {
  return new Promise((resolve) => {
    let changeSlideSeconds = Math.round(durationInSeconds / animation.length);
    let frame = 0;
    animate();

    function animate() {
      if (frame === animation.length) {
        display.clear();
        resolve();
        return;
      }

      const pixels = renderPixels(
        animation[frame].display,
        animation[frame].color,
        animation[frame].background);

      display.setPixels(pixels);
      frame++;
      
      setTimeout(animate, changeSlideSeconds * 1000);
    }
  });
}

module.exports.renderPixels = renderPixels;
module.exports.renderAnimation = renderAnimation;