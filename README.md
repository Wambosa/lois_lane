# lois_lane
Writing reports for Perry White and the Daily Planet
  
  
Lois Lane understands json better than superman. Give her a json formatted input (that you aquire through your own nefarious means)and watch her write an excellent docX report that even simpletons can read and understand.  
  
Currently Lois assumes that your template document is .\Template\Design_Template.docx  
She creates a "new_file.docx" right next to the executable or your defined outfile  
  
Example:  
PS> .\LoisLane.exe -design-document -file test_input.json  
PS> .\LoisLane.exe -design-document -file test_input.json -outfile my_cool_name.docx  