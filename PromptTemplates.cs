using System;
using System.Collections.Generic;

namespace LMS
{
    public static class PromptTemplates
    {
        public const string DifficultyPlaceholder = "{difficulty}";
        public const string LanguagePlaceholder = "{language}";
        public const string ContentDomainPlaceholder = "{domain}";
        public const string MinimumQuestionsPlaceholder = "{min_questions}";
        public const string ExampleExam = "{ExampleExam}";
        public const string SourceDocument = "{SourceDocument}";

        public static string YesNoTemplate = @"
            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" HIGH-QUALITY ACADEMIC YES/NO QUESTIONS from " + SourceDocument + @".pdf.
            You are functioning as an advanced academic assessment generator with expertise in " + ContentDomainPlaceholder + @".
            FOCUS EXCLUSIVELY on substantive content in " + SourceDocument + @".pdf - ignore all metadata, front matter, indexes, appendices, and references.

            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core concepts, basic relationships)
            - Level 3-4: Advanced undergraduate (application, initial analysis)
            - Level 5-6: Masters-level graduate (complex analysis, synthesis)
            - Level 7-8: Doctoral/research level (theoretical integration, methodological evaluation)
            - Level 9-10: Expert practitioner/researcher (cutting-edge applications, theoretical contributions)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of concepts.
            For levels 3-4: Require thorough analysis and integration of related concepts.
            For levels 5-6: Demand synthesis across multiple frameworks and evaluation of competing perspectives.
            For levels 7-8: Necessitate critique of methodologies and theoretical reconciliation.
            For levels 9-10: Require expert judgment on emergent applications and theoretical extensions.

            [EXTRACTION IMPERATIVES]
            Extract fundamental frameworks, methodological approaches, empirical findings, and scholarly arguments directly from " + SourceDocument + @".pdf content.
            PRIORITIZE disciplinary intersections, methodological nuances, statistical implications, and theoretical extensions.

            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE YES/NO QUESTIONS with EXACTLY 50% 'Yes' answers and 50% 'No' answers from " + SourceDocument + @".pdf content.
            2. EVERY question MUST necessitate higher-order thinking: inference, concept integration, and critical evaluation.
            3. EACH answer MUST be explicitly supported with direct textual evidence from " + SourceDocument + @".pdf content.
            4. ALWAYS INCLUDE a comprehensive 'source' field with direct references from " + SourceDocument + @".pdf, including page number.
            5. PROVIDE an 'explanation' field that derives its content directly from " + SourceDocument + @".pdf, offering a detailed academic rationale.
            6. ENSURE questions distribute evenly across document sections to cover full content breadth as found in " + SourceDocument + @".pdf.
            7. ALL answers and explanations MUST be derived from " + SourceDocument + @".pdf content.
            8. QUESTIONS MUST HAVE RANDOMLY DISTRIBUTED 'Yes' AND 'No' ANSWERS - do not follow a predictable pattern like alternating Yes/No or grouping similar answers together. However, ensure the FIRST question must have an answer of 'Yes' and the LAST question must have an answer of 'No'.

            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted PRECISELY as:
            [
              {
                ""question"": ""Precise, unambiguous question requiring subject mastery"",
                ""answer"": ""Yes|No"",
                ""source"": ""SourceDocument.pdf (Page X)"",
                ""explanation"": ""Comprehensive academic rationale derived directly from SourceDocument.pdf"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": """ + ContentDomainPlaceholder + @"""
              }
            ]

            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @".
            MAINTAIN field-appropriate academic terminology and conventions consistent with " + ContentDomainPlaceholder + @".
            ";

        public static string OptionsTemplateOnePDF = @"
            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" RIGOROUS MULTIPLE-CHOICE ASSESSMENT ITEMS from " + SourceDocument + @".pdf.
            You are functioning as an advanced academic assessment designer with expertise in " + ContentDomainPlaceholder + @".
            FOCUS EXCLUSIVELY on substantive content in " + SourceDocument + @".pdf - ignore all metadata, front matter, indexes, appendices, and references.

            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core concepts, basic relationships)
            - Level 3-4: Advanced undergraduate (application, initial analysis)
            - Level 5-6: Masters-level graduate (complex analysis, synthesis)
            - Level 7-8: Doctoral/research level (theoretical integration, methodological evaluation)
            - Level 9-10: Expert practitioner/researcher (cutting-edge applications, theoretical contributions)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of concepts.
            For levels 3-4: Require thorough analysis and integration of related concepts.
            For levels 5-6: Demand synthesis across multiple frameworks and evaluation of competing perspectives.
            For levels 7-8: Necessitate critique of methodologies and theoretical reconciliation.
            For levels 9-10: Require expert judgment on emergent applications and theoretical extensions.

            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE MULTIPLE-CHOICE ASSESSMENT ITEMS with content extracted directly from " + SourceDocument + @".pdf.
            2. EACH question MUST include EXACTLY 4 options (A-D) with only ONE correct answer supported by " + SourceDocument + @".pdf content.
            3. ALL distractors MUST be plausible and based on common misconceptions from " + SourceDocument + @".pdf content.
            4. DISTRIBUTE correct answers EVENLY across options A, B, C, and D.
            5. INCLUDE a detailed 'source' field with specific evidence from " + SourceDocument + @".pdf (Page X).
            6. PROVIDE comprehensive 'explanation' that offers a rationale for the correct answer and analysis of distractors.
            7. ENSURE questions distribute evenly across sections in " + SourceDocument + @".pdf.
            8. ALL options MUST be relevant and derived from " + SourceDocument + @".pdf content.

            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted PRECISELY as:
            [
                {
                ""question"": ""Precise, unambiguous question requiring subject mastery"",
                ""answer"": ""Option A|Option B|Option C|Option D"",
                ""options"": [
                    ""Option A: Well-crafted response based on SourceDocument.pdf"",
                    ""Option B: Plausible distractor"",
                    ""Option C: Common misconception"",
                    ""Option D: Partial understanding""
                ],
                ""source"": ""SourceDocument.pdf (Page X)"",
                ""explanation"": ""Comprehensive rationale derived from SourceDocument.pdf"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": """ + ContentDomainPlaceholder + @"""
                }
            ]

            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @".
            MAINTAIN field-appropriate academic terminology and conventions consistent with " + ContentDomainPlaceholder + @".
            ";

        public static string OptionsTemplateTwoPDF = @"
            [DOCUMENT HANDLING]
            TWO PDFs WILL BE PROVIDED:
            1. " + ExampleExam + @".pdf - Contains the REQUIRED QUESTION FORMAT and STYLE to emulate
            2. " + SourceDocument + @".pdf - Contains the CONTENT RESERVOIR for question generation

            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" RIGOROUS MULTIPLE-CHOICE ASSESSMENT ITEMS from " + SourceDocument + @".pdf.
            You are functioning as an advanced academic assessment designer with expertise in " + ContentDomainPlaceholder + @".
            FOCUS EXCLUSIVELY on substantive content in " + SourceDocument + @".pdf - ignore all metadata, front matter, indexes, appendices, and references.

            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core concepts, basic relationships)
            - Level 3-4: Advanced undergraduate (application, initial analysis)
            - Level 5-6: Masters-level graduate (complex analysis, synthesis)
            - Level 7-8: Doctoral/research level (theoretical integration, methodological evaluation)
            - Level 9-10: Expert practitioner/researcher (cutting-edge applications, theoretical contributions)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of concepts.
            For levels 3-4: Require thorough analysis and integration of related concepts.
            For levels 5-6: Demand synthesis across multiple frameworks and evaluation of competing perspectives.
            For levels 7-8: Necessitate critique of methodologies and theoretical reconciliation.
            For levels 9-10: Require expert judgment on emergent applications and theoretical extensions.

            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE MULTIPLE-CHOICE ASSESSMENT ITEMS with content extracted directly from " + SourceDocument + @".pdf.
            2. EACH question MUST include EXACTLY 4 options (A-D) with only ONE correct answer that is directly supported by " + SourceDocument + @".pdf content.
            3. ALL distractors (incorrect options) MUST be plausible and based on common misconceptions or partial understandings relevant to " + SourceDocument + @".pdf content.
            4. DISTRIBUTE correct answers PRECISELY and EVENLY across options A, B, C, and D.
            5. INCLUDE a detailed 'source' field with specific textual evidence from " + SourceDocument + @".pdf (Page X) that supports both the question and its correct answer.
            6. PROVIDE comprehensive 'explanation' that offers a rationale for why the correct answer is right and why each distractor is wrong, all directly sourced from " + SourceDocument + @".pdf.
            7. ENSURE questions distribute evenly across document sections to cover full content breadth as found in " + SourceDocument + @".pdf.
            8. ALL options (correct and distractors) MUST be relevant to the question and derived from " + SourceDocument + @".pdf content.

            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted PRECISELY as shown in " + ExampleExam + @".pdf:
            [
              {
                ""question"": ""Precise, unambiguous question requiring subject mastery from " + SourceDocument + @".pdf"",
                ""answer"": ""Option A|Option B|Option C|Option D"",
                ""options"": [
                  ""Option A: well-crafted response with disciplinary precision based on " + SourceDocument + @".pdf content"",
                  ""Option B: well-crafted response with disciplinary precision based on " + SourceDocument + @".pdf content"",
                  ""Option C: well-crafted response with disciplinary precision based on " + SourceDocument + @".pdf content"",
                  ""Option D: well-crafted response with disciplinary precision based on " + SourceDocument + @".pdf content""
                ],
                ""source"": ""Direct textual evidence extracted from " + SourceDocument + @".pdf (Page X)"",
                ""explanation"": ""Comprehensive academic rationale for the correct answer and analysis of each distractor, all derived from " + SourceDocument + @".pdf content"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": ""Subject classification""
              },
              ...
            ]

            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @".
            MAINTAIN field-appropriate academic terminology, precise language, and scholarly discourse conventions as consistent with " + ContentDomainPlaceholder + @".
            ";

        public static string MathOptionsTemplateTwoPDF = @"
            [DOCUMENT HANDLING]
            TWO PDFs WILL BE PROVIDED:
            1. " + ExampleExam + @".pdf - Contains the REQUIRED QUESTION FORMAT, STYLE, and DIFFICULTY PROGRESSION to mirror
            2. " + SourceDocument + @".pdf - Contains the CONTENT RESERVOIR and COGNITIVE COMPLEXITY BENCHMARKS for question generation
            [MATHEMATICAL CONTENT HANDLING]
            1. Preserve LaTeX formatting conventions from " + ExampleExam + @".pdf (\$equation\$, \\[display\\], etc.) while mirroring source document's mathematical syntax
            2. Substitute numerical values maintaining equation integrity and source document's parameter ranges
            3. Include step-by-step explanations matching " + ExampleExam + @".pdf's solution narrative style
            4. Verify all formulas against " + SourceDocument + @".pdf content and " + ExampleExam + @".pdf's pedagogical patterns
            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" MULTIPLE-CHOICE ITEMS THAT:
            - Replicate " + ExampleExam + @".pdf's QUESTION STRUCTURE and ANSWER DISTRIBUTION
            - Maintain " + SourceDocument + @".pdf's CONCEPTUAL DENSITY and TECHNICAL PRECISION
            - Mirror the BALANCED DIFFICULTY PROGRESSION between " + ExampleExam + @".pdf and " + SourceDocument + @".pdf
            - Ensure parity in MATHEMATICAL NOTATION STYLES between both documents
            - Maintain SOURCE DOCUMENT's PROPORTIONAL EMPHASIS on different content areas
            - Preserve EXAMPLE EXAM's QUESTION-TO-CONTENT DENSITY RATIO
            - Align with SOURCE DOCUMENT's AXIOMATIC FRAMEWORKS and THEORETICAL FOUNDATIONS
            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core concepts, basic relationships)
            - Level 3-4: Advanced undergraduate (application, initial analysis)
            - Level 5-6: Masters-level graduate (complex analysis, synthesis)
            - Level 7-8: Doctoral/research level (theoretical integration, methodological evaluation)
            - Level 9-10: Expert practitioner/researcher (cutting-edge applications, theoretical contributions)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of concepts.
            For levels 3-4: Require thorough analysis and integration of related concepts.
            For levels 5-6: Demand synthesis across multiple frameworks and evaluation of competing perspectives.
            For levels 7-8: Necessitate critique of methodologies and theoretical reconciliation.
            For levels 9-10: Require expert judgment on emergent applications and theoretical extensions.
            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE MULTIPLE-CHOICE ASSESSMENT ITEMS with content extracted directly from " + SourceDocument + @".pdf.
            2. EACH question MUST include EXACTLY 4 options (A-D) with only ONE correct answer that is directly supported by " + SourceDocument + @".pdf content.
            3. ALL distractors (incorrect options) MUST be plausible and based on common misconceptions or partial understandings relevant to " + SourceDocument + @".pdf content.
            4. DISTRIBUTE correct answers PRECISELY and EVENLY across options A, B, C, and D.
            5. INCLUDE a detailed 'source' field with specific textual evidence from " + SourceDocument + @".pdf (Page X) that supports both the question and its correct answer.
            6. PROVIDE comprehensive 'explanation' that offers a rationale for why the correct answer is right and why each distractor is wrong, all directly sourced from " + SourceDocument + @".pdf.
            7. ENSURE questions distribute evenly across document sections to cover full content breadth as found in " + SourceDocument + @".pdf.
            8. ALL options (correct and distractors) MUST be relevant to the question and derived from " + SourceDocument + @".pdf content.
            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted PRECISELY as shown in " + ExampleExam + @".pdf:
            [
              {
                ""question"": ""Precise, unambiguous question requiring subject mastery from " + SourceDocument + @".pdf"",
                ""answer"": ""Option A|Option B|Option C|Option D"",
                ""options"": [
                  ""Option A: well-crafted response with disciplinary precision based on " + SourceDocument + @".pdf content"",
                  ""Option B: well-crafted response with disciplinary precision based on " + SourceDocument + @".pdf content"",
                  ""Option C: well-crafted response with disciplinary precision based on " + SourceDocument + @".pdf content"",
                  ""Option D: well-crafted response with disciplinary precision based on " + SourceDocument + @".pdf content""
                ],
                ""source"": ""Direct textual evidence extracted from " + SourceDocument + @".pdf (Page X)"",
                ""explanation"": ""Comprehensive academic rationale for the correct answer and analysis of each distractor, all derived from " + SourceDocument + @".pdf content"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": ""Subject classification""
              },
              ...
            ]

            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @".
            MAINTAIN field-appropriate academic terminology, precise language, and scholarly discourse conventions as consistent with " + ContentDomainPlaceholder + @".
            ";

        public static string ProgrammingOptionsTemplateTwoPDF = @"
            [DOCUMENT HANDLING]
            TWO PDFs WILL BE PROVIDED:
            1. " + ExampleExam + @".pdf - Contains FORMAT STANDARDS, CODE CONVENTIONS, and PROBLEM-SOLVING PATTERNS to replicate
            2. " + SourceDocument + @".pdf - Provides ALGORITHMIC CONTENT and DEBUGGING SCENARIOS with AUTHENTIC COMPLEXITY
            [PROGRAMMING CONTENT HANDLING]
            1. Preserve code formatting from " + ExampleExam + @".pdf (syntax highlighting, indentation) while matching source document's implementation style
            2. Generate questions mirroring " + ExampleExam + @".pdf's ERROR PATTERNS and " + SourceDocument + @".pdf's DEBUGGING CONTEXTS
            3. Create distractors based on " + ExampleExam + @".pdf's COMMON MISTAKES and " + SourceDocument + @".pdf's EDGE CASES
            4. Validate all code constructs against both documents' technical specifications
            5. Maintain parity in CODE COMPLEXITY PROGRESSION between example exam and source material
            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" MULTIPLE-CHOICE ITEMS THAT:
            - Mirror EXAMPLE EXAM's CODE SNIPPET LENGTH and COMPLEXITY DISTRIBUTION
            - Preserve SOURCE DOCUMENT's ALGORITHMIC PARADIGM EMPHASIS
            - Maintain BALANCED REPRESENTATION of different programming constructs
            - Align with both documents' ERROR TAXONOMY CLASSIFICATIONS
            - Reflect SOURCE DOCUMENT's PERFORMANCE ANALYSIS CRITERIA
            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core concepts, basic relationships)
            - Level 3-4: Advanced undergraduate (application, initial analysis)
            - Level 5-6: Masters-level graduate (complex analysis, synthesis)
            - Level 7-8: Doctoral/research level (theoretical integration, methodological evaluation)
            - Level 9-10: Expert practitioner/researcher (cutting-edge applications, theoretical contributions)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of concepts.
            For levels 3-4: Require thorough analysis and integration of related concepts.
            For levels 5-6: Demand synthesis across multiple frameworks and evaluation of competing perspectives.
            For levels 7-8: Necessitate critique of methodologies and theoretical reconciliation.
            For levels 9-10: Require expert judgment on emergent applications and theoretical extensions.
            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE MULTIPLE-CHOICE ASSESSMENT ITEMS with content extracted directly from " + SourceDocument + @".pdf.
            2. EACH question MUST include EXACTLY 4 options (A-D) with only ONE correct answer that is directly supported by " + SourceDocument + @".pdf content.
            3. ALL distractors (incorrect options) MUST be plausible and based on common misconceptions or partial understandings relevant to " + SourceDocument + @".pdf content.
            4. DISTRIBUTE correct answers PRECISELY and EVENLY across options A, B, C, and D.
            5. INCLUDE a detailed 'source' field with specific textual evidence from " + SourceDocument + @".pdf (Page X) that supports both the question and its correct answer.
            6. PROVIDE comprehensive 'explanation' that offers a rationale for why the correct answer is right and why each distractor is wrong, all directly sourced from " + SourceDocument + @".pdf.
            7. ENSURE questions distribute evenly across document sections to cover full content breadth as found in " + SourceDocument + @".pdf.
            8. ALL options (correct and distractors) MUST be relevant to the question and derived from " + SourceDocument + @".pdf content.
            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted as:
            [
              {
                ""question"": ""Precise, unambiguous programming question requiring subject mastery from " + SourceDocument + @".pdf"",
                ""answer"": ""Option A|Option B|Option C|Option D"",
                ""options"": [
                  ""Option A: Correct implementation pattern"",
                  ""Option B: Common syntax/logic error"",
                  ""Option C: Suboptimal algorithm choice"",
                  ""Option D: Edge case oversight""
                ],
                ""source"": ""Direct textual evidence extracted from " + SourceDocument + @".pdf (Page X)"",
                ""explanation"": ""Comprehensive code analysis with debugging rationale"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": ""Subject classification""
              },
              ...
            ]
            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @".
            MAINTAIN field-appropriate programming terminology and code formatting conventions as consistent with " + ContentDomainPlaceholder + @".
            ";

        public static string MathOptionsTemplateOnePDF = @"
            [DOCUMENT HANDLING]
            ONLY ONE PDF WILL BE PROVIDED:
            1. " + SourceDocument + @".pdf - Contains BOTH CONTENT and FORMAT standards for question generation
            [MATHEMATICAL CONTENT HANDLING]
            1. Preserve LaTeX formatting conventions (\$equation\$, \\[display\\], etc.)
            2. Substitute numerical values while maintaining equation integrity
            3. Include step-by-step calculation explanations for solutions
            4. Verify all formulas against " + SourceDocument + @".pdf content
            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" RIGOROUS MULTIPLE-CHOICE ASSESSMENT ITEMS from " + SourceDocument + @".pdf.
            You are functioning as an advanced academic assessment designer with expertise in " + ContentDomainPlaceholder + @".
            FOCUS EXCLUSIVELY on substantive mathematical content in " + SourceDocument + @".pdf - ignore all metadata, front matter, indexes, appendices, and references.
            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core concepts, basic relationships)
            - Level 3-4: Advanced undergraduate (application, initial analysis)
            - Level 5-6: Masters-level graduate (complex analysis, synthesis)
            - Level 7-8: Doctoral/research level (theoretical integration, methodological evaluation)
            - Level 9-10: Expert practitioner/researcher (cutting-edge applications, theoretical contributions)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of concepts.
            For levels 3-4: Require thorough analysis and integration of related concepts.
            For levels 5-6: Demand synthesis across multiple frameworks and evaluation of competing perspectives.
            For levels 7-8: Necessitate critique of methodologies and theoretical reconciliation.
            For levels 9-10: Require expert judgment on emergent applications and theoretical extensions.
            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE MULTIPLE-CHOICE ASSESSMENT ITEMS with content extracted directly from " + SourceDocument + @".pdf.
            2. EACH question MUST include EXACTLY 4 options (A-D) with only ONE correct answer that is directly supported by " + SourceDocument + @".pdf content.
            3. ALL distractors (incorrect options) MUST be plausible and based on common misconceptions or partial understandings relevant to " + SourceDocument + @".pdf content.
            4. DISTRIBUTE correct answers PRECISELY and EVENLY across options A, B, C, and D.
            5. INCLUDE a detailed 'source' field with specific textual evidence from " + SourceDocument + @".pdf (Page X) that supports both the question and its correct answer.
            6. PROVIDE comprehensive 'explanation' that offers a rationale for why the correct answer is right and why each distractor is wrong, all directly sourced from " + SourceDocument + @".pdf.
            7. ENSURE questions distribute evenly across document sections to cover full content breadth as found in " + SourceDocument + @".pdf.
            8. ALL options (correct and distractors) MUST be relevant to the question and derived from " + SourceDocument + @".pdf content.
            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted as:
            [
              {
                ""question"": ""Precise, unambiguous question requiring subject mastery"",
                ""answer"": ""Option A|Option B|Option C|Option D"",
                ""options"": [
                  ""Option A: Mathematically precise response"",
                  ""Option B: Alternative plausible response"",
                  ""Option C: Common misconception"",
                  ""Option D: Partial understanding""
                ],
                ""source"": """ + SourceDocument + @".pdf (Page X)"",
                ""explanation"": ""Step-by-step solution with mathematical justification"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": """ + ContentDomainPlaceholder + @"""
              }
            ]
            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @".
            MAINTAIN field-appropriate academic terminology and mathematical notation conventions.
            ";

        public static string ProgrammingOptionsTemplateOnePDF = @"
            [DOCUMENT HANDLING]
            ONLY ONE PDF WILL BE PROVIDED:
            1. " + SourceDocument + @".pdf - Contains BOTH CONTENT and FORMAT standards for question generation
            [PROGRAMMING CONTENT HANDLING]
            1. Preserve code formatting conventions (syntax highlighting, indentation)
            2. Generate questions about algorithm implementation and debugging
            3. Include code snippet-based distractors with common syntax/logic errors
            4. Verify all programming constructs against " + SourceDocument + @".pdf content
            [EXTRACTION DIRECTIVE]
            GENERATE A MINIMUM OF " + MinimumQuestionsPlaceholder + @" RIGOROUS MULTIPLE-CHOICE ASSESSMENT ITEMS from " + SourceDocument + @".pdf.
            You are functioning as an advanced academic assessment designer with expertise in " + ContentDomainPlaceholder + @".
            FOCUS EXCLUSIVELY on substantive programming content in " + SourceDocument + @".pdf - ignore all metadata, front matter, indexes, appendices, and references.
            [MANDATORY DIFFICULTY CALIBRATION: " + DifficultyPlaceholder + @"/10]
            Difficulty spectrum:
            - Level 1-2: Foundation undergraduate (core concepts, basic relationships)
            - Level 3-4: Advanced undergraduate (application, initial analysis)
            - Level 5-6: Masters-level graduate (complex analysis, synthesis)
            - Level 7-8: Doctoral/research level (theoretical integration, methodological evaluation)
            - Level 9-10: Expert practitioner/researcher (cutting-edge applications, theoretical contributions)
            [DIFFICULTY ENFORCEMENT REQUIREMENT]
            FORCE STRICT ADHERENCE TO THE SPECIFIED DIFFICULTY LEVEL (" + DifficultyPlaceholder + @"/10) - DO NOT PRODUCE ANY QUESTION WITH A LOWER DIFFICULTY LEVEL.

            [COGNITIVE COMPLEXITY REQUIREMENTS]
            For levels 1-2: Test comprehension and basic application of concepts.
            For levels 3-4: Require thorough analysis and integration of related concepts.
            For levels 5-6: Demand synthesis across multiple frameworks and evaluation of competing perspectives.
            For levels 7-8: Necessitate critique of methodologies and theoretical reconciliation.
            For levels 9-10: Require expert judgment on emergent applications and theoretical extensions.
            [NON-NEGOTIABLE QUESTION FORMULATION REQUIREMENTS]
            1. GENERATE AT MINIMUM " + MinimumQuestionsPlaceholder + @" UNIQUE MULTIPLE-CHOICE ASSESSMENT ITEMS with content extracted directly from " + SourceDocument + @".pdf.
            2. EACH question MUST include EXACTLY 4 options (A-D) with only ONE correct answer that is directly supported by " + SourceDocument + @".pdf content.
            3. ALL distractors (incorrect options) MUST be plausible and based on common misconceptions or partial understandings relevant to " + SourceDocument + @".pdf content.
            4. DISTRIBUTE correct answers PRECISELY and EVENLY across options A, B, C, and D.
            5. INCLUDE a detailed 'source' field with specific textual evidence from " + SourceDocument + @".pdf (Page X) that supports both the question and its correct answer.
            6. PROVIDE comprehensive 'explanation' that offers a rationale for why the correct answer is right and why each distractor is wrong, all directly sourced from " + SourceDocument + @".pdf.
            7. ENSURE questions distribute evenly across document sections to cover full content breadth as found in " + SourceDocument + @".pdf.
            8. ALL options (correct and distractors) MUST be relevant to the question and derived from " + SourceDocument + @".pdf content.
            [MANDATORY OUTPUT FORMAT]
            The answer ONE Option
            Return a valid, parseable JSON array with objects formatted as:
            [
              {
                ""question"": ""Precise, unambiguous programming question"",
                ""answer"": ""Option A|Option B|Option C|Option D"",
                ""options"": [
                  ""Option A: Correct code implementation"",
                  ""Option B: Common syntax error"",
                  ""Option C: Logic flaw"",
                  ""Option D: Inefficient approach""
                ],
                ""source"": """ + SourceDocument + @".pdf (Page X)"",
                ""explanation"": ""Code analysis with debugging rationale"",
                ""difficulty"": """ + DifficultyPlaceholder + @"/10"",
                ""domain"": """ + ContentDomainPlaceholder + @"""
              }
            ]
            [LANGUAGE AND STYLISTIC REQUIREMENTS]
            PRODUCE ALL content in " + LanguagePlaceholder + @".
            MAINTAIN field-appropriate programming terminology and code formatting conventions as consistent with " + ContentDomainPlaceholder + @".
            ";
    }
}
