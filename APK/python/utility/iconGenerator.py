from PIL import Image

filename = input('input PNG filename: ')
img = Image.open(filename + '.png')
img.save(filename + '.ico', 'ico')

icon_sizes = [(16,16), (32, 32), (48, 48), (64,64)]
img.save(filename + '.ico', 'ico', sizes=icon_sizes)