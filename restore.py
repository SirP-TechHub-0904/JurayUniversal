import json
import os

transcript_path = r'C:\Users\JURAY\.gemini\antigravity-ide\brain\fb25e25d-56fd-418a-82ac-f799bc7655a8\.system_generated\logs\transcript_full.jsonl'

target_files = [
    r'c:\Users\JURAY\source\repos\JurayUniversal\src\JurayUniversal.Website\Areas\MisAlumni\Pages\Platform\Opportunities.cshtml'.lower(),
    r'c:\Users\JURAY\source\repos\JurayUniversal\src\JurayUniversal.Website\Areas\MisAlumni\Pages\Platform\MyProfile.cshtml'.lower(),
    r'c:\Users\JURAY\source\repos\JurayUniversal\src\JurayUniversal.Website\Areas\MisAlumni\Pages\Platform\Join.cshtml'.lower(),
    r'c:\Users\JURAY\source\repos\JurayUniversal\src\JurayUniversal.Website\Areas\MisAlumni\Pages\Platform\Index.cshtml'.lower(),
    r'c:\Users\JURAY\source\repos\JurayUniversal\src\JurayUniversal.Website\Areas\MisAlumni\Pages\Platform\Directory.cshtml'.lower(),
    r'c:\Users\JURAY\source\repos\JurayUniversal\src\JurayUniversal.Website\Areas\MisReports\Pages\Platform\Index.cshtml'.lower(),
    r'c:\Users\JURAY\source\repos\JurayUniversal\src\JurayUniversal.Website\Areas\MisReports\Pages\Platform\Submit.cshtml'.lower()
]

restored = {}

with open(transcript_path, 'r', encoding='utf-8') as f:
    for line in f:
        try:
            data = json.loads(line)
            if 'tool_calls' in data:
                for call in data['tool_calls']:
                    if call['name'] == 'write_to_file':
                        target = call['args'].get('TargetFile', '').lower()
                        for t_file in target_files:
                            if t_file in target:
                                restored[t_file] = call['args']['CodeContent']
        except Exception as e:
            pass

for f_path, content in restored.items():
    # Fix the layout
    content = content.replace('/Pages/Shared/Layouts/MIS/_MisUserLayout.cshtml', '/Pages/Shared/Admin/_User.cshtml')
    with open(f_path, 'w', encoding='utf-8') as out_f:
        out_f.write(content)
    print(f"Restored {f_path}")
