import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FileProcessService } from '@/services/file-process.service';

interface FileWithLoadingState {
  file: File;
  isLoading: boolean;
}

@Component({
  selector: 'app-file-processor',
  imports: [CommonModule],
  templateUrl: './file-processor.component.html',
  styleUrl: './file-processor.component.css'
})
export class FileProcessorComponent {
  selectedFiles: FileWithLoadingState[] = [];

  constructor(private fileProcessService: FileProcessService) { }

  onFilesSelected(event: Event): void {
    const input = event.target as HTMLInputElement;

    // Check if any file is selected
    if (input.files && input.files.length > 0) {
      this.selectedFiles = Array.from(input.files).filter(
        (file) => file.type === "text/plain"
      ).map(file => ({ file, isLoading: false }));

      // check if any file is not a text file
      if (this.selectedFiles.length === 0) {
        alert("Please select only text files.");
        input.value = '';
      }
    }
  }

  processFiles(): void {
    // Check if any file is selected
    if (this.selectedFiles.length === 0) {
      alert("Please select files to upload.");
      return;
    }

    // Process each file
    this.selectedFiles.forEach((fileWithState) => {
      fileWithState.isLoading = true; // Set loading state to true
      this.fileProcessService.processFile(fileWithState.file).subscribe(
      {
        next: (response) => {
          console.log(`File ${fileWithState.file.name} processed successfully.`);
          fileWithState.isLoading = false; // Reset loading state
        },
        error: (error) => {
          console.error(`Error processing file ${fileWithState.file.name}:`, error);
          fileWithState.isLoading = false; // Reset loading state
        }
      });
    });
  }
}
