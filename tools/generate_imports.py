import shutil
from os import listdir, remove, mkdir
from os.path import isfile, join, exists

# List of ignored dlls (in lowercase)
ignore = [
  'openal32.dll',
]

# Path settings
in_path = '../bin/net8.0'
out_path = '../bin/net8.0'
out_directory = 'engine'
out_full_path = f'{out_path}/{out_directory}/'

# Archive settings
archive_name = f'{out_directory}'
archive_type = 'zip'
archive_directory = f'{out_path}/'
archive_location = f'{archive_directory}{archive_name}.{archive_type}'

# Import file settings
import_file_name = 'EngineImports.props'
import_file_location = f'{out_full_path}{import_file_name}'

# Getting all files
files = [file for file in listdir(in_path) if isfile(join(in_path, file))]

# Remove previous build
if exists(out_full_path):
  shutil.rmtree(out_full_path)

if exists(archive_location):
  remove(archive_location)

# Creating new dictionary
mkdir(f'{out_path}/{out_directory}')

# Copy files and generate
import_content = '<Project>\n  <ItemGroup>\n'
for file in files:
  if not file.endswith('.dll'):
    continue

  if file.lower() in ignore:
    continue

  file_name = file.replace('.dll', '')
  import_content += f'    <Reference Include="{file_name}">\n      <HintPath>..\engine\{file}</HintPath>\n    </Reference>\n'
  shutil.copy2(f'{in_path}/{file}', f'{out_full_path}{file}')
import_content += '  </ItemGroup>\n</Project>'

# Create import file
with open(import_file_location, 'w') as file:
  file.write(import_content)

# Create archive
shutil.make_archive(f'{archive_directory}{archive_name}', archive_type, archive_directory)