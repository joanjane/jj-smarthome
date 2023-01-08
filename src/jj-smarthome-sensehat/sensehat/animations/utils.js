export function renderPixels(display, color, background) {
  return display.map(p => p === 'x' ? color : background);
}

export function renderAnimation(display, durationInSeconds, animation) {
  return new Promise((resolve) => {
    let changeSlideSeconds = Math.round(durationInSeconds / animation.length);
    let frame = 0;
    const interval = setInterval(() => {
      const pixels = renderPixels(
        animation[frame].display,
        animation[frame].color,
        animation[frame].background);

      display.setPixels(pixels);

      if (durationInSeconds === 0) {
        clearInterval(interval);
        resolve();
        return;
      }

      if (durationInSeconds % changeSlideSeconds === 0 && frame < animation.length) {
        frame++;
      }
      durationInSeconds--;
    }, durationInSeconds * 1000);
  });
}