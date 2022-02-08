import Vue from 'vue';

Vue.directive('numeric-input', {
  bind(el) {
    el.addEventListener('keypress', ($event) => {
      let charCode = $event.which ? $event.which : $event.keyCode;
      if (
        charCode > 31 &&
        (charCode < 48 || charCode > 57) &&
        charCode !== 46
      ) {
        $event.preventDefault();
      } else {
        return true;
      }
    });
  },
});
