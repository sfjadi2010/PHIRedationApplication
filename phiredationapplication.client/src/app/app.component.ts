import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FileProcessorComponent } from "./components/file-processor/file-processor.component";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [CommonModule, FileProcessorComponent]
})
export class AppComponent {

  constructor() {}

}
