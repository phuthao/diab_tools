<template>
  <div
    class="image-input image-input-outline image-input-empty"
    id="kt_image_5"
    ref="kt_image_5"
    style="background-image: url(assets/media/users/blank.png)"
  >
    <div class="image-input-wrapper"></div>

    <label
      class="btn btn-xs btn-icon btn-circle btn-white btn-hover-text-primary btn-shadow"
      data-action="change"
      data-toggle="tooltip"
      title=""
      data-original-title="Change avatar"
    >
      <i class="fa fa-pen icon-sm text-muted"></i>
      <input type="file" name="profile_avatar" accept=".png, .jpg, .jpeg" />
      <input type="hidden" name="profile_avatar_remove" />
    </label>

    <span
      class="btn btn-xs btn-icon btn-circle btn-white btn-hover-text-primary btn-shadow"
      data-action="cancel"
      data-toggle="tooltip"
      title="Cancel avatar"
    >
      <i class="ki ki-bold-close icon-xs text-muted"></i>
    </span>

    <span
      class="btn btn-xs btn-icon btn-circle btn-white btn-hover-text-primary btn-shadow"
      data-action="remove"
      data-toggle="tooltip"
      title="Remove avatar"
    >
      <i class="ki ki-bold-close icon-xs text-muted"></i>
    </span>
  </div>
</template>

<script>
import KTImageInput from '@/assets/js/components/image-input.js';

export default {
  mounted() {
    var avatar5 = new KTImageInput('kt_image_5');

    avatar5.on('cancel', function () {});

    avatar5.on('change', function () {});

    avatar5.on('remove', function () {});
  },
};
</script>
