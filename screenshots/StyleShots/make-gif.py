# Assembles an animated figure from the frames StyleShots renders. WPF can
# write a multi-frame GIF but not the per-frame delays or the loop extension,
# so the encoding happens here instead.
#
#   dotnet run --project screenshots/StyleShots -- input/docs --frames=<tmp>
#   python screenshots/StyleShots/make-gif.py <tmp>/progressring      input/docs/controls/images/progressring.gif
#   python screenshots/StyleShots/make-gif.py <tmp>/metroprogressbar  input/docs/controls/images/metroprogressbar-indeterminate.gif
#   python screenshots/StyleShots/make-gif.py <tmp>/progressbar       input/docs/styles/images/progressbar-indeterminate.gif
#
# Requires Pillow. Each frame set covers exactly one turn of its storyboard, so
# the loop is seamless and every frame gets the same delay.

import sys
from pathlib import Path

from PIL import Image

DELAY_MS = 50

# 128 is where the gradient in the ProgressBar stripes stops banding; going to
# the full 256 buys nothing and costs 4 KB.
COLORS = 128


def main(frame_dir: str, target: str) -> None:
    paths = sorted(Path(frame_dir).glob("frame*.png"))
    if not paths:
        raise SystemExit(f"no frames in {frame_dir}")

    frames = [Image.open(p).convert("RGB") for p in paths]

    # One palette for the whole animation: quantising each frame on its own
    # makes the dots shimmer as the palette shifts under them. Derive it from
    # every frame stacked together, not from the first one - an animation that
    # starts on an empty frame would otherwise get a palette with no ink in it
    # and come out blank.
    width, height = frames[0].size
    montage = Image.new("RGB", (width, height * len(frames)))
    for index, frame in enumerate(frames):
        montage.paste(frame, (0, index * height))

    palette = montage.quantize(colors=COLORS, method=Image.MEDIANCUT)
    frames = [f.quantize(palette=palette, dither=Image.Dither.NONE) for f in frames]

    frames[0].save(
        target,
        save_all=True,
        append_images=frames[1:],
        duration=DELAY_MS,
        loop=0,
        optimize=True,
    )

    size = Path(target).stat().st_size
    print(f"{target}  {frames[0].width}x{frames[0].height}  "
          f"{len(frames)} frames  {size / 1024:.0f} KB")


if __name__ == "__main__":
    if len(sys.argv) != 3:
        raise SystemExit("usage: make-progressring-gif.py <frame-dir> <target.gif>")
    main(sys.argv[1], sys.argv[2])
