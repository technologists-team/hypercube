import sys
import os
from PIL import Image

def embed_image_to_csharp(input_path: str, output_path: str):
    if not os.path.exists(input_path):
        print(f"Ошибка: Файл '{input_path}' не найден.")
        return

    try:
        # 1. Получаем размеры изображения
        with Image.open(input_path) as img:
            width, height = img.size
        
        # 2. Читаем сырые байты PNG файла
        with open(input_path, 'rb') as f:
            file_bytes = f.read()
        
        # 3. Форматируем байты в HEX-представление для C# (0x00, 0xFF, ...)
        # Это самый надежный и читаемый способ хранения бинарных данных в коде
        byte_string = ", ".join(f"0x{b:02X}" for b in file_bytes)
        
        # 4. Записываем результат в файл (ровно 2 строки)
        with open(output_path, 'w', encoding='utf-8') as f:
            f.write(f"{width}x{height}\n")
            f.write(f"[ {byte_string} ]\n")
            
        print(f"✅ Успешно!")
        print(f"📏 Размер: {width}x{height}")
        print(f"💾 Байт: {len(file_bytes)}")
        print(f"📄 Результат сохранен в: {output_path}")
        
    except Exception as e:
        print(f"❌ Произошла ошибка: {e}")

if __name__ == "__main__":
    # Проверка аргументов командной строки
    if len(sys.argv) != 3:
        print("Использование: python embed_image.py <путь_к_изображению.png> <путь_к_выходному_файлу.txt>")
        print("Пример: python embed_image.py icon.png embedded_icon.txt")
    else:
        embed_image_to_csharp(sys.argv[1], sys.argv[2])