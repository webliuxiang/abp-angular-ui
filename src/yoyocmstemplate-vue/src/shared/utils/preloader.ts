export function preloaderFinished() {
  const body = document.getElementsByTagName('body')[0];
  const preloader = document.getElementsByClassName('preloader')[0];

  body.style.overflow = 'hidden';

  function remove() {
    // preloader value null when running --hmr
    if (!preloader) {
      return;
    }
    preloader.addEventListener('transitionend', () => {
      preloader.className = 'preloader-hidden';
    });

    preloader.className += ' preloader-hidden-add preloader-hidden-add-active';
  }

  setTimeout(() => {
    remove();
    body.style.overflow = '';
  }, 100);

}
